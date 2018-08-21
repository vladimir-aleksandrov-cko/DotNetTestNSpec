namespace DotNetTestNSpec.Configuration
{
    public class AdapterSettingsDto
    {
        public string Delimiter1 { set; get; }
        public string Delimiter2 { set; get; }

        public AdapterSettings ToAdapterSettings()
        {
            return new AdapterSettings(Delimiter1, Delimiter2);
        }
    }
}