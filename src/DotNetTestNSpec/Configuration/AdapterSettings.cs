using System.Xml;

namespace DotNetTestNSpec.Configuration
{
    public class AdapterSettings
    {
        public readonly string Delimiter1;
        public readonly string Delimiter2;

        public AdapterSettings(string delimiter1, string delimiter2)
        {
            Delimiter1 = delimiter1;
            Delimiter2 = delimiter2;
        }
    }
}