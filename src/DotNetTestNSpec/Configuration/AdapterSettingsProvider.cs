using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using static DotNetTestNSpec.Domain.Constants;


namespace DotNetTestNSpec.Configuration
{
    [SettingsName(RunSettingsXmlNode)]
    public class AdapterSettingsProvider : IAdapterSettingsProvider
    {
        public AdapterSettings Settings { get; private set; }
        public void Load(XmlReader reader)
        {
            Settings = AdapterSettings.Default;

            if (reader == null)
            {
                return;
            }

            if (reader.Read() && reader.Name == RunSettingsXmlNode)
            {
                var serializer = new XmlSerializer(typeof(AdapterSettingsDto));
                Settings = (serializer.Deserialize(reader) as AdapterSettingsDto)?.ToAdapterSettings()
                            ?? AdapterSettings.Default;
            }
        }
    }
}