using NSpec;
using NSpec.Domain;

namespace DotNetTestNSpec.Domain
{
    public class ContextFinder
    {
        public ContextCollection BuildContextCollection(string binaryPath)
        {
            var reflector = new Reflector(binaryPath);

            var finder = new SpecFinder(reflector);

            var conventions = new DefaultConventions();

            var contextBuilder = new ContextBuilder(finder, conventions);

            var contextCollection = contextBuilder.Contexts();

            contextCollection.Build();

            return contextCollection;
        }
    }
}