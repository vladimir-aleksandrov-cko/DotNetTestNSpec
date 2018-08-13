using System.Collections.Generic;
using System.Linq;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace DotNetTestNSpec.Api
{
    [FileExtension(Constants.DllExtension)]
    [FileExtension(Constants.ExeExtension)]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
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

            logger.Info($"{nameof(DiscoverTests)} for sources: {string.Join(";", sources)}");

            foreach (var binaryPath in sources)
            {
                logger.Info($"Discovering tests in {binaryPath}");

                var examples = controller.List(binaryPath);
            }
        }
    }
}