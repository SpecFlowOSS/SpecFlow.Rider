using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.ExecutionFailedStep
{
    [StaticSeverityHighlighting(Severity.INFO, typeof(ExecutionFailedStepGutterMarks), OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false)]
    public class ExecutionFailedStepHighlighting : IHighlighting
    {
        public string ToolTip => "This step failed during last test execution";
        public string ErrorStripeToolTip => ToolTip;

        private readonly GherkinStep _gherkinStep;

        public ExecutionFailedStepHighlighting(GherkinStep gherkinStep)
        {
            _gherkinStep = gherkinStep;
        }

        public bool IsValid()
        {
            return true;
        }

        public DocumentRange CalculateRange()
        {
            return _gherkinStep.GetDocumentRange();
        }
    }
}
