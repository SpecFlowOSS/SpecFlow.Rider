using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using JetBrains.ReSharper.UnitTestFramework.Highlighting;
using ReSharperPlugin.SpecflowRiderPlugin.Helpers;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.TestsGutterMarks
{
    public class TestsGutterMarksDaemonProcess : IDaemonStageProcess
    {
        private readonly GherkinFile _gherkinFile;
        private readonly IUnitTestElementManager _unitTestElementManager;
        public IDaemonProcess DaemonProcess { get; }

        public TestsGutterMarksDaemonProcess(
            IDaemonProcess daemonProcess,
            GherkinFile gherkinFile,
            IUnitTestElementManager unitTestElementManager
        )
        {
            _gherkinFile = gherkinFile;
            _unitTestElementManager = unitTestElementManager;
            DaemonProcess = daemonProcess;
        }

        public void Execute(Action<DaemonStageResult> committer)
        {
            var highlightings = new List<HighlightingInfo>();
            foreach (var gherkinFeature in _gherkinFile.GetFeatures())
            {
                var childrenUnitTests = new List<IUnitTestElement>();
                IUnitTestElement parent = null;
                foreach (var gherkinScenario in gherkinFeature.GetScenarios())
                {
                    // FIXME: Search in feature and iterate only over children
                    var unitTestElement = _unitTestElementManager.UnitTestElements.FirstOrDefault(e => UnitTestHelpers.IsTestElementForScenario(e, gherkinFeature.GetFeatureText(), gherkinScenario.GetScenarioText()));
                    if (unitTestElement == null)
                        continue;

                    parent = unitTestElement.Parent;

                    var scenarioHighlighting = new SpecflowUnitTestHighlighting(
                        _gherkinFile.GetSolution(),
                        unitTestElement,
                        new List<IUnitTestElement>(),
                        null,
                        gherkinScenario.GetDocumentRange()
                    );

                    highlightings.Add(new HighlightingInfo(gherkinScenario.GetDocumentRange(), scenarioHighlighting));
                    childrenUnitTests.Add(unitTestElement);
                }

                if (parent != null)
                {
                    var featureHighlighting = new SpecflowUnitTestHighlighting(
                        _gherkinFile.GetSolution(),
                        parent,
                        new List<IUnitTestElement>(),
                        null,
                        gherkinFeature.GetDocumentRange()
                    );
                    highlightings.Add(new HighlightingInfo(gherkinFeature.GetDocumentRange(), featureHighlighting));
                }
            }
            committer(new DaemonStageResult(highlightings));
        }
    }
}