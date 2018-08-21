using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using static DotNetTestNSpec.Domain.Constants;


namespace DotNetTestNSpec.Configuration
{
    // TODO: Taken from NSpec.VsAdapter
    public class AdapterSettingsProvider : IAdapterSettingsProvider
    {
        public AdapterSettings Settings { get; private set; }
        public void Load(XmlReader reader)
        {
            if (reader == null)
            {
                Settings = new AdapterSettings("22", "22");
                return;
            }

            try
            {
                if (reader.Read() && reader.Name == RunSettingsXmlNode)
                {
                    var serializer = new XmlSerializer(typeof(AdapterSettingsDto));
                    Settings = (serializer.Deserialize(reader) as AdapterSettingsDto)?.ToAdapterSettings() ?? new AdapterSettings("", "");
                }
                else
                {
                    Settings = new AdapterSettings("33", "33");
                    return;
                }
            }
            catch (Exception)
            {
                // Swallow exception, probably cannot even log at this time
                Settings = new AdapterSettings("44", "44");
            }
        }
    }
}