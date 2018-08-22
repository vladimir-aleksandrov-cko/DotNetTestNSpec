using System.Collections.Generic;
using NSpec.Domain;

namespace DotNetTestNSpec.Domain
{
    public class ExampleFinder
    {
        public IEnumerable<ExampleBase> Find(string binaryPath)
        {
            var contextFinder = new ContextFinder();

            var contexts = contextFinder.BuildContextCollection(binaryPath);

            var examples = contexts.Examples();

            return examples;
        }
    }
}