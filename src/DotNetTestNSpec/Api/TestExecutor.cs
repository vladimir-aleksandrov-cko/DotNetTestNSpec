using System;
using System.Collections.Generic;
using System.Linq;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
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

            var supportedProperties =
              new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase)
              {
                  [TestCaseProperties.FullyQualifiedName.Label] = TestCaseProperties.FullyQualifiedName
              };

            var filterExpression = runContext
                .GetTestCaseFilter(supportedProperties.Keys, (propertyName) =>
                {
                    TestProperty testProperty = null;
                    supportedProperties.TryGetValue(propertyName, out testProperty);
                    return testProperty;
                });

            // The adapter can then query if the test case has been filtered out using the following snippet.



            logger.Info($"Using Mode {settings.Mode}");
            logger.Info($"{nameof(RunTests)} for sources: {string.Join(";", sources)}");

            foreach (var binaryPath in sources)
            {
                if (isCancelled)
                {
                    break;
                }

                logger.Info($"Running tests in {binaryPath}");
                var tests = binaryDiscoverer.Discover(binaryPath).Where(FilterTestCase)
                        .ToLookup(t => t.FullyQualifiedName, t => t);

                bool FilterTestCase(TestCase testCase)
                {
                    var result = filterExpression.MatchTestCase(testCase, (propertyName) =>
                        {
                            if (!supportedProperties.TryGetValue(propertyName, out var testProperty))
                            {
                                logger.Warning($"Property {propertyName} is not supported");
                                return null;
                            }

                            if (settings.Mode == DisplayNameMode.VSCode
                                && testProperty == TestCaseProperties.FullyQualifiedName)
                            {
                                return testCase.GetPropertyValue(TestCaseProperties.DisplayName);
                            }

                            return testCase.GetPropertyValue(testProperty);
                        });

                    logger.Warning($"TestCase {testCase.DisplayName} matched: {result}. FQDN: {testCase.FullyQualifiedName}");

                    return result;
                }

                var testsList = tests.Select(k => k.Key).ToList();

                logger.Warning($"# Tests to run:{testsList.Count}, {testsList.FirstOrDefault()}");


            ExampleRunner.




                nspecController.RunInteractive(
                    binaryPath,
                    testsList,
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
                        try
                        {
                            var testOutcome = ex.ToTestOutcome();
                            var test = tests[ex.FullName].First();
                            frameworkHandle.RecordEnd(test, ex.ToTestOutcome());
                            frameworkHandle.RecordResult(new TestResult(test) { Outcome = testOutcome });
                        }
                        catch (Exception exc)
                        {
                            logger.Error($"Failed to record {ex.FullName}: {exc.Message}");
                        }
                    });
            }
        }
    }
}