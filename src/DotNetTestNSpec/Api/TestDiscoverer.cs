using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using static DotNetTestNSpec.Domain.Constants;

namespace DotNetTestNSpec.Api
{
    [FileExtension(DllExtension)]
    [FileExtension(ExeExtension)]
    [DefaultExecutorUri(ExecutorUriString)]
    public class TestDiscoverer : ITestDiscoverer
    {
        /// <summary>
        /// Discovers the tests available from the provided containers (DLLs)
        /// </summary>
        /// <param name="sources">Collection of test containers (DLLs)</param>
        /// <param name="discoveryContext">Context in which discovery is being performed (run settings)</param>
        /// <param name="messageLogger">Logger used to log messages.</param>
        /// <param name="discoverySink">Used to send testcases and discovery related events back to Discoverer manager.</param>
        public void DiscoverTests(
            IEnumerable<string> sources,
            IDiscoveryContext discoveryContext,
            IMessageLogger messageLogger,
            ITestCaseDiscoverySink discoverySink)
        {
            var logger = new TestLogger(messageLogger);
            var controller = new NspecController();

            var settingsProvider = discoveryContext.RunSettings.GetSettings(RunSettingsXmlNode) as IAdapterSettingsProvider;

            var settings = settingsProvider?.Settings ?? new AdapterSettings("11", "11");

            logger.Info($"Settings1={settings.Delimiter1}, Settings2={settings.Delimiter2}");

            logger.Info($"{nameof(DiscoverTests)} for sources: {string.Join(";", sources)}");

            foreach (var binaryPath in sources)
            {
                logger.Info($"Discovering tests in {binaryPath}");

                foreach (var example in controller.List(binaryPath))
                {
                    var testCase = example.ToTestCase();
                    logger.Info("Found TestCase --- BEGIN");
                    logger.Info("FQDN: " + testCase.FullyQualifiedName);
                    logger.Info("ExecutorUri: " + testCase.ExecutorUri);
                    logger.Info("Source: " + testCase.Source);
                    logger.Info("DisplayName: " + testCase.DisplayName);
                    logger.Info("CodeFilePath: " + testCase.DisplayName);
                    logger.Info("LineNumber: " + testCase.LineNumber);
                    logger.Info("Found TestCase --- END");
                    discoverySink.SendTestCase(testCase);
                }
            }
        }
    }
}