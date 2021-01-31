using JetBrains.Annotations;

namespace ReSharperPlugin.SpecflowRiderPlugin.Psi
{
    public class GherkinFeature : GherkinElement
    {
        public GherkinFeature() : base(GherkinNodeTypes.FEATURE)
        {
        }

        public string GetFeatureText()
        {
            return this.FindChild<GherkinToken>(o => o.NodeType == GherkinTokenTypes.TEXT)?.GetText();
        }

        [CanBeNull]
        public GherkinScenario GetScenario(string scenarioText)
        {
            return this.FindChild<GherkinScenario>(o => o.GetScenarioText() == scenarioText);
        }
    }
}