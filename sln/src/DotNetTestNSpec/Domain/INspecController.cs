using System.Collections.Generic;
using DotNetTestNSpec.Domain.Library;
using NSpec.Api.Discovery;

namespace DotNetTestNSpec.Domain
{
    // Interface for NSpec.Api.Wrapper
    public interface INspecController
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
