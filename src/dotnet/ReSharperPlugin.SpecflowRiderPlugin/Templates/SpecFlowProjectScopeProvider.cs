using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Context;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    [ShellComponent]
    public class SpecFlowProjectScopeProvider : ScopeProvider
    {

        public SpecFlowProjectScopeProvider()
        {
            // These factory methods are used to create scope points when reading templates from settings
            Creators.Add(TryToCreate<FeatureFileTemplateSectionMarker>);
        }

        public override IEnumerable<ITemplateScopePoint> ProvideScopePoints(TemplateAcceptanceContext context)
        {
            var sourceFile = context.SourceFile;
            if (sourceFile == null)
                yield break;

            yield return new FeatureFileTemplateSectionMarker();
        }
    }
}