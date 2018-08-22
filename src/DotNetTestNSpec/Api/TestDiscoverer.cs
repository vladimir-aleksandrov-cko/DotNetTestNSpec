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

            var settingsProvider = discoveryContext.RunSettings.GetSettings(RunSettingsXmlNode) as IAdapterSettingsProvider;

            var settings = settingsProvider?.Settings ?? AdapterSettings.Default;

            var discoverer = new BinaryPathDiscoverer(logger, settings);

            logger.Info($"Using Mode {settings.Mode}");
            logger.Info($"{nameof(DiscoverTests)} for sources: {string.Join(";", sources)}");

            foreach (var binaryPath in sources)
            {
                logger.Info($"Discovering tests in {binaryPath}");

                foreach (var testCase in discoverer.Discover(binaryPath))
                {
                    logger.Info("Found TestCase --- BEGIN");
                    logger.Info(testCase.ToString());
                    logger.Info("Found TestCase --- END");
                    discoverySink.SendTestCase(testCase);
                }
            }
        }
    }
}