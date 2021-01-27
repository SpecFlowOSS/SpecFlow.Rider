using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.SyntaxHighlighting;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.ReSharper
{
    [Language(typeof(GherkinLanguage))]
    public class GherkinSyntaxHighlightingManager : SyntaxHighlightingManager
    {
        public override SyntaxHighlightingStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings, IFile getPrimaryPsiFile)
        {
            return new GherkinSyntaxHighlightingStageProcess(process, settings, getPrimaryPsiFile, new GherkinSyntaxHighlightingProcessor());
        }
    }
}