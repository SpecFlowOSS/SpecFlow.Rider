using System.Collections.Generic;
using System.Drawing;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Highlighting;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.TestsGutterMarks
{
    [StaticSeverityHighlighting(Severity.INFO, typeof(UnitTestGutterMarks), OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
    internal class SpecflowUnitTestHighlighting :
        IUnitTestElementHighlighting,
        IRichTextToolTipHighlighting
    {
        // ReSharper disable InconsistentNaming
        public const string HIGHLIGHTER_ATTRIBUTE_BROKEN_ELEMENT = "ReSharper Broken Unit Test Element";
        public const string HIGHLIGHTER_ATTRIBUTE_ELEMENT = "ReSharper Unit Test Element";
        public const string HIGHLIGHTER_ATTRIBUTE_ELEMENTS = "ReSharper Unit Test Element Container";
        public const string HIGHLIGHTER_ATTRIBUTE_CONTAINER_SUCCESSFUL = "ReSharper Unit Test Container Successful";
        public const string HIGHLIGHTER_ATTRIBUTE_ELEMENT_SUCCESSFUL = "ReSharper Unit Test Element Successful";
        public const string HIGHLIGHTER_ATTRIBUTE_CONTAINER_FAILED = "ReSharper Unit Test Container Failed";
        public const string HIGHLIGHTER_ATTRIBUTE_ELEMENT_FAILED = "ReSharper Unit Test Element Failed";
        public const string HIGHLIGHTER_ATTRIBUTE_CONTAINER_IGNORED = "ReSharper Unit Test Container Ignored";
        public const string HIGHLIGHTER_ATTRIBUTE_ELEMENT_IGNORED = "ReSharper Unit Test Element Ignored";
        // ReSharper restore InconsistentNaming

        private readonly ISolution mySolution;
        private readonly DocumentRange myRange;

        public SpecflowUnitTestHighlighting(
            ISolution solution,
            IUnitTestElement element,
            IList<IUnitTestElement> subElements,
            IUnitTestElement superElement,
            DocumentRange range)
        {
            mySolution = solution;
            Element = element;
            SubElements = subElements;
            SuperElement = superElement;
            myRange = range;
        }

        public DocumentRange CalculateRange() => myRange;

        public string AttributeId
        {
            get
            {
                var result = mySolution.GetComponent<IUnitTestResultManager>().GetResult(Element);
                var flag = Element.IsOfKind(UnitTestElementKind.TestContainer);
                if (!result.Outdated)
                {
                    if (result.Status.Has(UnitTestStatus.Failed))
                        return !flag ? HIGHLIGHTER_ATTRIBUTE_ELEMENT_FAILED : HIGHLIGHTER_ATTRIBUTE_CONTAINER_FAILED;
                    if (result.Status.Has(UnitTestStatus.Success))
                        return !flag ? HIGHLIGHTER_ATTRIBUTE_ELEMENT_SUCCESSFUL : HIGHLIGHTER_ATTRIBUTE_CONTAINER_SUCCESSFUL;
                    if (result.Status.Has(UnitTestStatus.Ignored | UnitTestStatus.Inconclusive))
                        return !flag ? HIGHLIGHTER_ATTRIBUTE_ELEMENT_IGNORED : HIGHLIGHTER_ATTRIBUTE_CONTAINER_IGNORED;
                }
                return !flag ? HIGHLIGHTER_ATTRIBUTE_ELEMENT : HIGHLIGHTER_ATTRIBUTE_ELEMENTS;
            }
        }

        public string ErrorStripeToolTip => null;

        public string ToolTip => TryGetTooltip(HighlighterTooltipKind.TextEditor)?.Text;

        /// <inheritdoc />
        public RichTextBlock TryGetTooltip(HighlighterTooltipKind where)
        {
            var result = mySolution.GetComponent<IUnitTestResultManager>().GetResult(Element);
            if (result.Status.Has(UnitTestStatus.Success))
                return Render("passed", null);
            var status = result.Status;
            if (status.Has(UnitTestStatus.Failed))
                return Render("failed", result);
            status = result.Status;
            return status.Has(UnitTestStatus.Ignored | UnitTestStatus.Inconclusive) ? Render("skipped", result) : Render(null, null);

            RichTextBlock Render(string outcome, UnitTestResult resultIfShown)
            {
                var richtext = new RichText(Element.Kind, TextStyle.Default);
                if (!outcome.IsNullOrWhitespace())
                    richtext.Append(' ', TextStyle.Default).Append(outcome.Trim(), TextStyle.Default);
                richtext.Append(' ', TextStyle.Default).Append("(Click to run)", TextStyle.FromForeColor(Color.Silver));
                var richTextBlock = new RichTextBlock(richtext);
                if (resultIfShown != null)
                {
                    var shortMessage = result.ShortMessage;
                    if (shortMessage != null && !shortMessage.IsEmpty())
                        richTextBlock.Add(shortMessage);
                }
                return richTextBlock;
            }
        }

        public IUnitTestElement Element { get; }

        public IList<IUnitTestElement> SubElements { get; }

        public IUnitTestElement SuperElement { get; }

        public bool IsValid() => true;
    }
}