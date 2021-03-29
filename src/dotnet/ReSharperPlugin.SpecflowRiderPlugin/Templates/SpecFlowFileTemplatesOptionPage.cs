using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.IDE.UI;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.ReSharper.Host.Features.Settings.OptionsPage;
using JetBrains.ReSharper.LiveTemplates.UI;
using JetBrains.ReSharper.Psi.EditorConfig;
using JetBrains.ReSharper.Resources.Resources.Icons;
using JetBrains.Rider.Model;
using JetBrains.UI.ThemedIcons;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    [ZoneMarker(typeof(IRiderModelZone))]
    [OptionsPage(PID, "Gherkin", typeof(SpecFlowThemedIcons.Specflow))]
    public class SpecFlowFileTemplatesOptionPage : RiderFileTemplatesOptionPageBase
    {
        
        public const string PID = "RiderFeatureFileTemplatesSettings";

        public SpecFlowFileTemplatesOptionPage(Lifetime lifetime,
                                               SpecFlowProjectScopeCategoryUIProvider uiProvider,
                                               OptionsPageContext optionsPageContext,
                                               OptionsSettingsSmartContext optionsSettingsSmartContext,
                                               StoredTemplatesProvider storedTemplatesProvider,
                                               ScopeCategoryManager scopeCategoryManager,
                                               IDialogHost dialogHost,
                                               TemplatesUIFactory uiFactory, IconHostBase iconHostBase)
            : base(lifetime, uiProvider, optionsPageContext, optionsSettingsSmartContext, storedTemplatesProvider, scopeCategoryManager,
                uiFactory, iconHostBase, dialogHost, "Gherkin") {
        }
        
    }
}