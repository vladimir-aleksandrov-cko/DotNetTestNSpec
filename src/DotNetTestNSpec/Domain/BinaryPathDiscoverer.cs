using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json;
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
                    var foundAsyncNavData = false;

                    var navigationData = diaSession.GetNavigationData(specClassName, exampleMethodName);

                    if (string.IsNullOrWhiteSpace(navigationData?.FileName) || navigationData?.MaxLineNumber == int.MaxValue)
                    {
                        var stateMachineClassName = AsyncMethodHelper.GetClassNameForAsyncMethod(assembly, specClassName, exampleMethodName);

                        if (stateMachineClassName != null)
                        {
                            navigationData = diaSession.GetNavigationData(stateMachineClassName, "MoveNext");
                            foundAsyncNavData = true;
                        }
                    }

                    string displayName;

                    if (_runSettings.Mode == DisplayNameMode.Beautify)
                    {
                        displayName = example.FullName().Beautify();
                    }

                    else
                    {
                        // We need to append a method name to make Net Explorer Extension find a symbol
                        var testPostfix = exampleMethodName;
                        var match = Regex.Match(exampleMethodName, @"<(?<methodName>\S+)>");
                        if (match.Success)
                        {
                            testPostfix = match.Groups["methodName"].Value;
                        }

                        // We also need to replace white spaces with underscore, to make tests executable
                        // Tests are exected by passing fqdn
                        displayName = $"{example.FullName().Trim().Replace(' ', '_')}{testPostfix}";
                    }


                    var exampleInfo = new
                    {
                        FullName = example.FullName(),
                        Async = example.IsAsync,
                        foundAsyncNavData = foundAsyncNavData,
                        Type = example.GetType().Name,
                        MethodName = example.BodyMethodInfo.Name,
                        FileName = navigationData.FileName,
                        MinLineNumber = navigationData.MinLineNumber,
                        MaxLineNumber = navigationData.MaxLineNumber,
                        SpecClassName = specClassName,
                    };

                    _logger.Info(JsonConvert.SerializeObject(exampleInfo));


                    yield return new TestCase
                    {
                        FullyQualifiedName = example.FullName(),
                        DisplayName = displayName,
                        Source = binaryPath,
                        CodeFilePath = navigationData?.FileName,
                        LineNumber = navigationData?.MinLineNumber ?? 0,
                        ExecutorUri = new Uri(ExecutorUriString),
                        Id = Guid.NewGuid()
                    };
                }
            }
        }
    }
}