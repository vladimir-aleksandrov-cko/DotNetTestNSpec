using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Api.Discovery;
using NSpec.Domain;

using static DotNetTestNSpec.Domain.Constants;

namespace DotNetTestNSpec.Domain
{
    public class BinaryPathDiscoverer
    {
        private readonly ITestLogger _logger;
        private readonly AdapterSettings _runSettings;

        public BinaryPathDiscoverer(ITestLogger logger, AdapterSettings runSettings)
        {
            _logger = logger;
            _runSettings = runSettings;
        }

        public IEnumerable<TestCase> Discover(string binaryPath)
        {
            using (var diaSession = new DiaSession(binaryPath))
            {
                Assembly assembly = null;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(binaryPath);
                }
                catch (Exception exc)
                {
                    _logger.Error($"Failed to load assembly from {binaryPath}: {exc.Message}");
                    yield break;
                }

                var examples = new ExampleFinder().Find(binaryPath);

                foreach (var example in examples)
                {
                    var methodInfo = example.BodyMethodInfo;
                    var specClassName = methodInfo.DeclaringType.FullName;
                    var exampleMethodName = methodInfo.Name;

                    var navigationData = diaSession.GetNavigationData(specClassName, exampleMethodName);

                    if (string.IsNullOrWhiteSpace(navigationData?.FileName) || navigationData?.MaxLineNumber == int.MaxValue)
                    {
                        var stateMachineClassName = AsyncMethodHelper.GetClassNameForAsyncMethod(assembly, specClassName, exampleMethodName);

                        if (stateMachineClassName != null)
                        {
                            navigationData = diaSession.GetNavigationData(stateMachineClassName, "MoveNext");
                        }
                    }

                    string displayName;

                    if (_runSettings.Mode == DisplayNameMode.Beautify)
                    {
                        displayName = example.FullName().Beautify();
                    }

                    else
                    {
                        // We need to append a method name to make VSCode find a symbol
                        var testPostfix = exampleMethodName;
                        var match = Regex.Match(exampleMethodName, @"<(?<methodName>\S+)>");
                        if (match.Success)
                        {
                            testPostfix = match.Groups["methodName"].Value;
                        }
                        displayName = $"{example.FullName()}{testPostfix}";
                    }

                    yield return new TestCase
                    {
                        FullyQualifiedName = example.FullName(),
                        DisplayName = displayName,
                        Source = binaryPath,
                        CodeFilePath = navigationData?.FileName,
                        LineNumber = navigationData?.MinLineNumber ?? 0,
                        ExecutorUri = new Uri(ExecutorUriString)

                    };

                }

            }

        }
    }
}