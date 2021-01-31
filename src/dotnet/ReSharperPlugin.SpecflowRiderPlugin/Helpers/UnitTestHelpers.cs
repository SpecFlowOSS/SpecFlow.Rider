using System.Linq;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestFramework;
using ReSharperPlugin.SpecflowRiderPlugin.Extensions;

namespace ReSharperPlugin.SpecflowRiderPlugin.Helpers
{
    public static class UnitTestHelpers
    {
        private static readonly ClrTypeName NunitDescriptionAttribute = new ClrTypeName("NUnit.Framework.DescriptionAttribute");

        public static bool IsTestElementForScenario(IUnitTestElement testElement, string featureText, string scenarioText)
        {
            var declaredElement = testElement.GetDeclaredElement();
            if (declaredElement == null)
                return false;
            if (!(testElement.GetDeclaredElement() is IMethod methodTestDeclaration))
                return false;
            var psiSourceFile = declaredElement.GetSourceFiles().SingleItem;
            if (psiSourceFile?.Name.EndsWith(".feature.cs") != true)
                return false;

            var project = psiSourceFile.GetProject();
            var gherkinFile = project?.GetGherkinFile(psiSourceFile.Name.Substring(0, psiSourceFile.Name.Length - 3));

            var gherkinDocument = gherkinFile?.GetSourceFile()?.Document;
            if (gherkinDocument == null)
                return false;

            using (CompilationContextCookie.GetOrCreate(project.GetResolveContext()))
            {
                var scenarioAttributeDescription = methodTestDeclaration.GetAttributeInstances(NunitDescriptionAttribute, false).FirstOrDefault();
                if (scenarioAttributeDescription == null || scenarioAttributeDescription.PositionParameterCount < 1)
                    return false;
                if (scenarioText != scenarioAttributeDescription.PositionParameter(0).ConstantValue.Value as string)
                    return false;

                var featureAttributeDescription = methodTestDeclaration.GetContainingType()?.GetAttributeInstances(NunitDescriptionAttribute, false).FirstOrDefault();
                if (featureAttributeDescription == null || featureAttributeDescription.PositionParameterCount < 1)
                    return false;
                if (featureText != featureAttributeDescription.PositionParameter(0).ConstantValue.Value as string)
                    return false;
            }

            return true;
        }
    }
}