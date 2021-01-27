using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.SyntaxHighlighting;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;
using ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.ReSharper;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.ReSharper
{
    [DaemonStage(StagesBefore = new[] {typeof (SmartResolverStage)})]
    public class GherkinSyntaxHighlightingStage : IDaemonStage
    {
        public IEnumerable<IDaemonStageProcess> CreateProcess(
            IDaemonProcess process,
            IContextBoundSettingsStore settings,
            DaemonProcessKind processKind)
        {
            if (processKind != DaemonProcessKind.VISIBLE_DOCUMENT)
                return Enumerable.Empty<IDaemonStageProcess>();
            
            var psiServices = process.SourceFile.GetPsiServices();
            psiServices.Files.AssertAllDocumentAreCommitted();
            var highlightingManager = psiServices.LanguageManager.TryGetService<GherkinSyntaxHighlightingManager>(GherkinLanguage.Instance.NotNull());
            if (highlightingManager == null)
                return Enumerable.Empty<IDaemonStageProcess>();
            
            var psiFiles = psiServices.Files.GetPsiFiles<GherkinLanguage>(process.SourceFile, PsiLanguageCategories.All);
            var daemonStageProcessList = new List<IDaemonStageProcess>();
            foreach (var getPrimaryPsiFile in psiFiles)
            {
                var highlightingProcess = highlightingManager.CreateProcess(process, settings, getPrimaryPsiFile);
                if (highlightingProcess != null)
                    daemonStageProcessList.Add(highlightingProcess);
            }
            
            return daemonStageProcessList;
        }
    }
}