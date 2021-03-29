using System.IO;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    [ShellComponent]
    public class SpecFlowTemplatesDefaultSettings : IHaveDefaultSettingsStream
    {
        public Stream GetDefaultSettingsStream(Lifetime lifetime)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ReSharperPlugin.SpecflowRiderPlugin.Templates.FileTemplates.xml");
            Assertion.AssertNotNull(stream, "stream != null");
            lifetime.AddDispose(stream);
            return stream;
        }

        public string Name => "SpecFlow default FileTemplates";
    }
}