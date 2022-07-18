using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Impl.Shared.InjectedPsi;
using JetBrains.ReSharper.Psi.JavaScript;
using JetBrains.ReSharper.Psi.JavaScript.Impl.Tree;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl.JSon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Injected;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Text;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.InjectedLanguages;

[SolutionComponent]
[ZoneMarker(typeof(ILanguageJavaScriptZone))]
public class JsonInGherkinInjectedPsiProvider : GenericGherkinInjectedPsiProvider<InjectedJsonLanguage>
{
    public JsonInGherkinInjectedPsiProvider()
        : base(InjectedJsonLanguage.Instance, "json")
    {
    }

    protected override bool CanBeGeneratedNode(ITreeNode node) => node is JavaScriptFileBase;
}

[SolutionComponent]
public class XmlInGherkinInjectedPsiProvider : GenericGherkinInjectedPsiProvider<InjectedXmlLanguage>
{
    public XmlInGherkinInjectedPsiProvider()
        : base(InjectedXmlLanguage.Instance, "xml")
    {
    }

    protected override bool CanBeGeneratedNode(ITreeNode node) => node is XmlFile;
}

public abstract class GenericGherkinInjectedPsiProvider<TTargetLanguage> : IndependentInjectedPsiProvider
    where TTargetLanguage : PsiLanguageType
{
    public override bool ProvidedLanguageCanHaveNestedInjects => false;
    public override PsiLanguageType GeneratedLanguage => _targetLanguage ?? (PsiLanguageType)UnknownLanguage.Instance!;

    private readonly TTargetLanguage _targetLanguage;
    private readonly string _textType;

    protected GenericGherkinInjectedPsiProvider(TTargetLanguage targetLanguage, string textType)
    {
        _targetLanguage = targetLanguage;
        _textType = textType;
    }

    public override bool IsApplicable(PsiLanguageType originalLanguage) => originalLanguage.Is<GherkinLanguage>();
    public override bool IsApplicableToNode(ITreeNode node, IInjectedFileContext context) => node is GherkinPystring gherkinPystring && gherkinPystring.GetTextType() == _textType;

    public override IInjectedNodeContext CreateInjectedNodeContext(
        IInjectedFileContext fileContext,
        ITreeNode originalNode
    )
    {
        if (originalNode is not GherkinPystring gherkinPystring)
            return null;
        var pyStringText = gherkinPystring.GetTextForLanguageInjection(out var startOffset);
        var languageService = _targetLanguage.LanguageService();
        return languageService == null
            ? null
            : CreateInjectedFileAndContext(fileContext, originalNode, new StringBuffer(pyStringText), languageService,
                startOffset,
                startOffset + pyStringText.Length,
                generatedStartOffset: 0,
                generatedEndOffset: pyStringText.Length);
    }

    public override IInjectedNodeContext Regenerate(IndependentInjectedNodeContext nodeContext)
    {
#warning FIXME
        return nodeContext;
    }


    protected override bool CanBeOriginalNode(ITreeNode node) => node is GherkinPystring;
}