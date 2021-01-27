using JetBrains.TextControl.DocumentMarkup;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting
{
    public class SpecFlowHighlighterNamesProvider : PrefixBasedSettingsNamesProvider
    {
        public SpecFlowHighlighterNamesProvider()
            : base("ReSharper SpecFlow", "ReSharper.SpecFlow")
        {
        }

        public override string GetHighlighterTag(string attributeId)
        {
            return base.GetHighlighterTag(attributeId);
        }

        public override string GetExternalName(string attributeId)
        {
            var tag = base.GetHighlighterTag(attributeId);
            return tag;
        }
    }
}