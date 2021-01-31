using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.TestsGutterMarks
{
    [DaemonStage(StagesBefore = new Type[] {typeof(ResolveProccessDaemonStage)})]
    public class TestsGutterMarksDaemonStage : IDaemonStage
    {
        private readonly IPsiServices _psiServices;
        private readonly IUnitTestElementManager _unitTestElementManager;

        public TestsGutterMarksDaemonStage(IPsiServices psiServices, IUnitTestElementManager unitTestElementManager)
        {
            _psiServices = psiServices;
            _unitTestElementManager = unitTestElementManager;
        }

        public IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process,
            IContextBoundSettingsStore settings,
            DaemonProcessKind processKind)
        {
            if (processKind == DaemonProcessKind.VISIBLE_DOCUMENT)
            {
                foreach (var gherkinFile in process.SourceFile.GetPsiFiles<GherkinLanguage>())
                    yield return new TestsGutterMarksDaemonProcess(process, (GherkinFile) gherkinFile, _unitTestElementManager);
            }
        }
    }
}