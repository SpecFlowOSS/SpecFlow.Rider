using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Paths;
using JetBrains.Util;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Extensions
{
    public static class ProjectExtensions
    {
        [CanBeNull]
        public static ICSharpFile GetCSharpFile(this IProject project, string filename)
        {
            var sourceFileInProject = project.GetPsiSourceFileInProject(FileSystemPath.Parse(filename, FileSystemPathInternStrategy.INTERN));
            return sourceFileInProject?.GetPsiFiles<CSharpLanguage>().SafeOfType<ICSharpFile>().SingleOrDefault<ICSharpFile>();
        }

        [CanBeNull]
        public static GherkinFile GetGherkinFile(this IProject project, string filename)
        {
            var sourceFileInProject = project.GetPsiSourceFileInProject(FileSystemPath.Parse(filename, FileSystemPathInternStrategy.INTERN));
            return sourceFileInProject?.GetPsiFiles<GherkinLanguage>().SafeOfType<GherkinFile>().SingleOrDefault();
        }
    }
}