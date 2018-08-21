using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace DotNetTestNSpec.Configuration
{
    public interface IAdapterSettingsProvider : ISettingsProvider
    {
        AdapterSettings Settings { get; }
    }
}