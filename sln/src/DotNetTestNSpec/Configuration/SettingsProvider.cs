using System.Xml;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace DotNetTestNSpec.Api
{
    [SettingsName("NspecTestAdapter")]
    public class SettingsProvider : ISettingsProvider
    {
        
        public void Load(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}