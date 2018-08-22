using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.Domain;
using static DotNetTestNSpec.Domain.Constants;

namespace DotNetTestNSpec.Api
{
    [ExtensionUri(ExecutorUriString)]
    public class TestExecutor : ITestExecutor
    {
        private bool isCancelled;
        public void Cancel() => isCancelled = true;
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            throw new System.NotImplementedException();
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var logger = new TestLogger(frameworkHandle);

            var settingsProvider = runContext.RunSettings.GetSettings(RunSettingsXmlNode) as IAdapterSettingsProvider;

            var settings = settingsProvider?.Settings ?? AdapterSettings.Default;

            var nspecController = new NspecController();
            var binaryDiscoverer = new BinaryPathDiscoverer(logger, settings);


            logger.Info($"Using Mode {settings.Mode}");
            logger.Info($"{nameof(RunTests)} for sources: {string.Join(";", sources)}");

            foreach (var binaryPath in sources)
            {
                if (isCancelled)
                {
                    break;
                }

                logger.Info($"Running tests in {binaryPath}");
                var tests = binaryDiscoverer.Discover(binaryPath)
                        .ToLookup(t => t.FullyQualifiedName, t => t);

                nspecController.RunInteractive(
                    binaryPath,
                    tests.Select(k => k.Key),
                    ex =>
                    {
                        try
                        {
                            frameworkHandle.RecordStart(tests[ex.FullName].First());
                        }
                        catch (Exception exc)
                        {
                            logger.Error($"Not found {ex.FullName}: {exc.Message}");
                        }
                    }
                    ,
                    ex =>
                    {
                        var testOutcome = ex.ToTestOutcome();
                        var test = tests[ex.FullName].First();
                        frameworkHandle.RecordEnd(test, ex.ToTestOutcome());
                        frameworkHandle.RecordResult(new TestResult(test) { Outcome = testOutcome });
                    });
            }
        }
    }
}