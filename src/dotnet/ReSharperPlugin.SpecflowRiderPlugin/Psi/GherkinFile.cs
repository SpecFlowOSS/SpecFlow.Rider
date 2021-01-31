using JetBrains.Annotations;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;

namespace ReSharperPlugin.SpecflowRiderPlugin.Psi
{
    public class GherkinFile : FileElementBase
    {
        public override NodeType NodeType => GherkinNodeTypes.FILE;
        public override PsiLanguageType Language => GherkinLanguage.Instance.NotNull();

        public string FileName { get; }

        public GherkinFile(string fileName)
        {
            FileName = fileName;
        }

        [CanBeNull]
        public GherkinFeature GetFeature(string text)
        {
            for (var te = (TreeElement) FirstChild; te != null; te = te.nextSibling)
            {
                if (te is GherkinFeature feature)
                    if (feature.GetFeatureText() == text)
                        return feature;
            }
            return null;
        }

        public override string ToString()
        {
            return $"GherkinFile: {FileName}";
        }
    }
}