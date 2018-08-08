using System.Collections.Generic;
using NSpec.Api.Discovery;

namespace DotNetTestNSpec.Domain.Library
{
    // Interface for NSpec.Api.Wrapper
    public interface IController
    {
        int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast);

        IEnumerable<DiscoveredExample> List(string testAssemblyPath);

        void RunInteractive(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            IExecutionSink sink);
    }
}
