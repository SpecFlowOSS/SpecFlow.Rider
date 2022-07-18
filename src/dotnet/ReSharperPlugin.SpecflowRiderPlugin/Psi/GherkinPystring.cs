using System.Text;
using JetBrains.Annotations;

namespace ReSharperPlugin.SpecflowRiderPlugin.Psi
{
    public class GherkinPystring : GherkinElement
    {
        public GherkinPystring() : base(GherkinNodeTypes.PYSTRING)
        {
        }

        protected override string GetPresentableText()
        {
            return string.Empty;
        }

        [CanBeNull]
        public string GetTextType()
        {
            var secondToken = FirstChild?.NextSibling;
            if (secondToken?.NodeType == GherkinTokenTypes.PYSTRING_TEXT)
                return secondToken?.GetText();
            return null;
        }

        public string GetTextForLanguageInjection(out int startOffset)
        {
            var sb = new StringBuilder();
            startOffset = 0;

            // Skip first line
            var node = FirstChild;
            while (node != null)
            {
                startOffset += node.GetTextLength();
                if (node.NodeType == GherkinTokenTypes.NEW_LINE)
                    break;
                node = node.NextSibling;
            }

            node = node?.NextSibling;

            var positionLastEol = 0;
            while (node != null && node.NodeType != GherkinTokenTypes.PYSTRING)
            {
                sb.Append(node.GetText());

                if (node.NodeType == GherkinTokenTypes.NEW_LINE)
                    positionLastEol = sb.Length;
                
                node = node.NextSibling;
            }
            sb.Length = positionLastEol;


            return sb.ToString();
        }
    }
}