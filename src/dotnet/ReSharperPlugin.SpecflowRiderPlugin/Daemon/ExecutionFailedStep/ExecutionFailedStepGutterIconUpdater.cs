using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Threading;
using JetBrains.Collections;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Session;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.Util;
using JetBrains.Util.Collections;
using ReSharperPlugin.SpecflowRiderPlugin.Extensions;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;
using GroupingEvent = JetBrains.Threading.GroupingEvent;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.ExecutionFailedStep
{
    [SolutionComponent]
    public class ExecutionFailedStepGutterIconUpdater
    {
        private static readonly ClrTypeName NunitDescriptionAttribute = new ClrTypeName("NUnit.Framework.DescriptionAttribute");
        private readonly ISolution _solution;
        private readonly IDocumentMarkupManager _documentMarkupManager;
        private readonly ILogger _myLogger;
        private readonly IDictionary<IUnitTestElement, (IUnitTestSession session, UnitTestResult result)> _updatedUnitTests;
        private readonly IDictionary<IUnitTestElement, IHighlighter> _highlighters = new Dictionary<IUnitTestElement, IHighlighter>();
        private readonly GroupingEvent _myResultUpdated;
        private readonly IUnitTestResultManager _unitTestResultManager;

        public ExecutionFailedStepGutterIconUpdater(
            Lifetime lifetime,
            [NotNull] ISolution solution,
            [NotNull] IShellLocks shellLocks,
            [NotNull] IUnitTestResultManager resultManager,
            [NotNull] IDocumentMarkupManager documentMarkupManager,
            [NotNull] ILogger logger
        )
        {
            _myLogger = logger;
            _solution = solution;
            _documentMarkupManager = documentMarkupManager.NotNull("documentMarkupManager != null");
            _updatedUnitTests = new Dictionary<IUnitTestElement, (IUnitTestSession session, UnitTestResult result)>(UnitTestElement.EqualityComparer);
            _myResultUpdated = shellLocks.NotNull("shellLocks != null")
                .CreateGroupingEvent(lifetime, nameof(ExecutionFailedStepGutterIconUpdater) + "::ResultUpdated", 500.Milliseconds(), OnProcessUpdated);
            _unitTestResultManager = resultManager;
            resultManager.NotNull("resultManager != null").UnitTestResultUpdated.Advise(lifetime, OnUnitTestResultUpdated);
        }

        private void OnProcessUpdated()
        {
            IDictionary<IUnitTestElement, (IUnitTestSession session, UnitTestResult result)> set;
            lock (_updatedUnitTests)
            {
                set = _updatedUnitTests.ToDictionary(e => e.Key, e => e.Value);
                _updatedUnitTests.Clear();
            }
            using (_myLogger.UsingLogBracket(LoggingLevel.TRACE, "Updating gutter mark icons for {0} elements", (object) set.Count))
            {
                using (ReadLockCookie.Create())
                    UpdateIconsInActiveDocuments(set);
            }
        }

        private void UpdateIconsInActiveDocuments(IDictionary<IUnitTestElement, (IUnitTestSession session, UnitTestResult result)> updatedUnitTests)
        {
            foreach (var (testElement, (session, result)) in updatedUnitTests)
            {
                var declaredElement = testElement.GetDeclaredElement();
                if (declaredElement == null)
                    continue;
                if (!(testElement.GetDeclaredElement() is IMethod methodTestDeclaration))
                    continue;
                var psiSourceFile = declaredElement.GetSourceFiles().SingleItem;
                if (psiSourceFile?.Name.EndsWith(".feature.cs") != true)
                    continue;

                var project = psiSourceFile.GetProject();
                var gherkinFile = project?.GetGherkinFile(psiSourceFile.Name.Substring(0, psiSourceFile.Name.Length - 3));

                var gherkinDocument = gherkinFile?.GetSourceFile()?.Document;
                if (gherkinDocument == null)
                    continue;

                var markupModel = _documentMarkupManager.TryGetMarkupModel(gherkinDocument);
                if (!result.Status.GetResultStatus().Has(UnitTestStatus.Failed))
                {
                    if (_highlighters.TryGetValue(testElement, out var previousHighlighter))
                        markupModel?.RemoveHighlighter(previousHighlighter);
                    return;
                }

                string featureText;
                string scenarioText;

                using (CompilationContextCookie.GetOrCreate(project.GetResolveContext()))
                {
                    var scenarioAttributeDescription = methodTestDeclaration.GetAttributeInstances(NunitDescriptionAttribute, false).FirstOrDefault();
                    if (scenarioAttributeDescription == null || scenarioAttributeDescription.PositionParameterCount < 1)
                        continue;
                    var featureAttributeDescription = methodTestDeclaration.GetContainingType()?.GetAttributeInstances(NunitDescriptionAttribute, false).FirstOrDefault();
                    if (featureAttributeDescription == null || featureAttributeDescription.PositionParameterCount < 1)
                        continue;
                    featureText = featureAttributeDescription.PositionParameter(0).ConstantValue.Value as string;
                    scenarioText = scenarioAttributeDescription.PositionParameter(0).ConstantValue.Value as string;
                }

                var feature = gherkinFile.GetFeature(featureText);
                var scenario = feature?.GetScenario(scenarioText);
                if (scenario == null)
                    continue;

                var testResult = _unitTestResultManager.GetResultData(testElement, session);
                var steps = scenario.GetSteps().ToList();
                if (steps.Count == 0)
                    continue;

                var invalidStep = FindFailedStep(steps, testResult);
                if (invalidStep == null)
                    continue;

                var documentRange = invalidStep.GetDocumentRange();
                if (markupModel != null)
                {
                    if (_highlighters.TryGetValue(testElement, out var previousHighlighter))
                        markupModel.RemoveHighlighter(previousHighlighter);

                    var highlighting = new ExecutionFailedStepHighlighting(invalidStep);
                    var tooltipProvider = DaemonUtil.GetHighlighterTooltipProvider(highlighting, _solution);
                    var highlighter = markupModel.AddHighlighter(null, documentRange.TextRange, AreaType.EXACT_RANGE, -1, "SpecFlow Failed Step", ErrorStripeAttributes.Empty, tooltipProvider);
                    _highlighters[testElement] = highlighter;
                }
            }
        }
        private static GherkinStep FindFailedStep(List<GherkinStep> steps, UnitTestResultData testResult)
        {

            var stepIndex = 0;
            var currentStep = steps[stepIndex];
            var inStep = false;
            GherkinStep invalidStep = null;
            for (int i = 0; i < testResult.OutputChunks; i++)
            {
                var chunk = testResult.GetOutputChunk(i);
                var lines = chunk.SplitByNewLine();
                foreach (var line in lines)
                {
                    if (!inStep && line == currentStep.GetText())
                    {
                        stepIndex++;
                        inStep = true;
                    }
                    else if (inStep && line.StartsWith("-> done: "))
                    {
                        inStep = false;
                        currentStep = steps[stepIndex];
                    }
                    else if (inStep && line.StartsWith("-> error: "))
                    {
                        invalidStep = currentStep;
                        break;
                    }
                }
            }
            return invalidStep;
        }

        private void OnUnitTestResultUpdated(UnitTestResultEventArgs e)
        {
            if (e.Result.Status.Has(UnitTestStatus.Running | UnitTestStatus.Pending))
                return;
            lock (_updatedUnitTests)
                _updatedUnitTests[e.Element] = (e.Session, e.Result);
            _myResultUpdated.FireIncomingDontProlongate();
        }
    }
}