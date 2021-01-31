/*using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using JetBrains.Diagnostics;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.ClrLanguages;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.AttributeChecker;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using JetBrains.ReSharper.UnitTestFramework.Exploration;
using JetBrains.ReSharper.UnitTestProvider.nUnit.Common;
using JetBrains.ReSharper.UnitTestProvider.nUnit.v26;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.TestsGutterMarks
{
    [SolutionComponent]
    internal class SpecflowTestExplorer : IUnitTestExplorerFromFile
    {
        private readonly NUnitServiceProvider _myServiceProvider;
        private readonly IUnitTestElementIdFactory _myIdFactory;
        private readonly IUnitTestElementManager _myElementManager;
        private readonly IUnitTestAttributeChecker _myAttributeChecker;
        private readonly INUnitTypeOrValuePresenterFactory _myPresenterFactory;

        public IUnitTestProvider Provider { get; }

        public SpecflowTestExplorer(
            NUnitServiceProvider serviceProvider,
            IUnitTestElementManager elementManager,
            IUnitTestElementIdFactory idFactory,
            IUnitTestAttributeChecker attributeChecker,
            INUnitTypeOrValuePresenterFactory presenterFactory,
            ClrLanguagesKnown clrLanguagesKnown)
        {
            Provider = serviceProvider.Provider.NotNull("provider != null");
            _myServiceProvider = serviceProvider;
            _myIdFactory = idFactory;
            _myElementManager = elementManager;
            _myAttributeChecker = attributeChecker;
            _myPresenterFactory = presenterFactory;
        }

        public void ProcessFile(
            IFile psiFile,
            IUnitTestElementsObserver observer,
            Func<bool> interrupted)
        {
            if (!(psiFile is GherkinFile gherkinFile))
                return;
            TextRange textRange1 = declaration.GetNameDocumentRange().TextRange;
            TextRange textRange2 = declaration.GetDocumentRange().TextRange;

            if (declaration is IFunctionDeclaration functionDeclaration)
            {
                unitTestElement = (IUnitTestElement) this._myIdFactory.Create().GetOrCreateTest(string.Format("{0}.{1}{2}", (object) typeName, !containingType.GetClrName().Equals((object) typeName) ? (object) (containingType.ShortName + ".") : (object) string.Empty, (object) typeMember.ShortName), this.Project, this.myTargetFrameworkId, testFixtureElement, containingType.GetClrName().GetPersistent(), typeMember.ShortName, (IEnumerable<string>) this.CollectCategories((IDeclaredElement) typeMember), new Action<IUnitTestElement>(this.myObserver.OnUnitTestElementChanged));
                unitTestElementList = (IList<IUnitTestElement>) this.CreateTestElement(typeMember, functionDeclaration.DeclaredElement, (NUnitTestElement) unitTestElement, testFixtureElement);
            }


            var factory = new NUnitElementFactory(_myServiceProvider, UnitTestElementOrigin.Source, _myElementManager);
            var nunitFileExplorer = new NUnitFileExplorer(psiFile, factory, _myIdFactory, _myElementManager, _myAttributeChecker, observer, _myPresenterFactory, interrupted);
            psiFile.ProcessDescendants(nunitFileExplorer);
            if (unitTestElement == null)
                return;
            if (unitTestElementList != null)
                unitTestElementList = (IList<IUnitTestElement>) unitTestElementList.Distinct<IUnitTestElement>().ToList<IUnitTestElement>();
            if (!textRange1.IsValid || !textRange2.IsValid)
                return;

            observer.OnUnitTestElementDisposition(new UnitTestElementDisposition(
                unitTestElement,
                gherkinFile.GetSourceFile().ToProjectFile(),
                textRange1,
                textRange2,
                unitTestElementList
            ));

        }
    }
}*/