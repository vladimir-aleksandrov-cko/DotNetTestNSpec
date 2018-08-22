using System.Xml;

namespace DotNetTestNSpec.Configuration
{
    public class AdapterSettings
    {
        // Removes nspec. prefix, replaces default context separator ('.') with '>'
        // Default value: false
        public readonly DisplayNameMode Mode;

        public AdapterSettings(DisplayNameMode mode)
        {
            Mode = mode;
        }

        public static AdapterSettings Default = new AdapterSettings(Constants.DefaultDisplayNameMode);
    }
    public enum DisplayNameMode
    {
        // Integrates with extension .NET Test Explorer, 
        // by appending full name with method name (or parent method name for async method),
        // as .NET Test Explorer uses "Find Symbol" on the display node
        VSCode,
        // Removes nspec. prefix, replaces default context separator ('.') with '>'
        Beautify
    }
}