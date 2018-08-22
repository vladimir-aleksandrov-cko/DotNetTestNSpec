using System.Xml.Serialization;

using static DotNetTestNSpec.Domain.Constants;

namespace DotNetTestNSpec.Configuration
{
    [XmlRoot(RunSettingsXmlNode)]
    public class AdapterSettingsDto
    {
        public DisplayNameMode Mode { set; get; }

        public AdapterSettings ToAdapterSettings()
        {
            return new AdapterSettings(Mode);
        }
    }
}