using System.Collections.Generic;
using JetBrains.Application.Icons.ImageSourceIcons;
using JetBrains.Application.Parts;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Templates;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.UI.Icons;
using JetBrains.UI.ThemedIcons;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    // Defines a category for the UI, and the scope points that it includes
    [ScopeCategoryUIProvider(Priority = 29.0, ScopeFilter = ScopeFilter.Project)]
    public class SpecFlowProjectScopeCategoryUIProvider : ScopeCategoryUIProvider
    {
        static SpecFlowProjectScopeCategoryUIProvider()
        {
            // UnityCSharp requires its own icon rather than the generic C# icon because it's used as the group icon
            // for the UITag "Unity Class" menu item
            TemplateImage.Register("Gherkin", SpecFlowThemedIcons.Specflow.Id);
        }
        
        public override IEnumerable<ITemplateScopePoint> BuildAllPoints()
        {
            yield return new FeatureFileTemplateSectionMarker();
        }
        
        // Needs to be less than other priorities in R#'s built in ScopeCategoryUIProvider
        // to push it to the end of the list
        // private const int Priority = -200;
        
        public SpecFlowProjectScopeCategoryUIProvider()
            : base(SpecFlowThemedIcons.Specflow.Id)
        {
            // The main scope point is used to the UID of the QuickList for this category.
            // It does nothing unless there is also a QuickList stored in settings.
            MainPoint = new FeatureFileTemplateSectionMarker();
        }

        public override string CategoryCaption => "Gherkin";

        public override IconId Icon => SpecFlowThemedIcons.Specflow.Id;
    }
}