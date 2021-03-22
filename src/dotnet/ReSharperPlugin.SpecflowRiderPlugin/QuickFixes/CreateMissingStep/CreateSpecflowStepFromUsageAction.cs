using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.UI.Controls;
using JetBrains.Application.UI.Controls.JetPopupMenu;
using JetBrains.Application.UI.Icons.CommonThemedIcons;
using JetBrains.Collections;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Extensions.Properties;
using JetBrains.Lifetimes;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Navigation.NavigationExtensions;
using JetBrains.ReSharper.Features.Internal.Annotator;
using JetBrains.ReSharper.Features.Navigation.Features.CommandPrompt;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Resources;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Naming.Extentions;
using JetBrains.ReSharper.Psi.Naming.Impl;
using JetBrains.ReSharper.Psi.Naming.Settings;
using JetBrains.ReSharper.Psi.Resources;
using JetBrains.ReSharper.Psi.Transactions;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.TextControl;
using JetBrains.UI.RichText;
using JetBrains.Util;
using ReSharperPlugin.SpecflowRiderPlugin.Caching.StepsDefinitions;
using ReSharperPlugin.SpecflowRiderPlugin.Extensions;
using ReSharperPlugin.SpecflowRiderPlugin.Helpers;
using ReSharperPlugin.SpecflowRiderPlugin.Psi;
using ReSharperPlugin.SpecflowRiderPlugin.References;
using ReSharperPlugin.SpecflowRiderPlugin.Utils.Steps;

namespace ReSharperPlugin.SpecflowRiderPlugin.QuickFixes.CreateMissingStep
{
    public class CreateSpecflowStepFromUsageAction : IBulbAction
    {
        public string Text { get; } = "Create step";
        private readonly SpecflowStepDeclarationReference _reference;
        private readonly IDialogHost _dialogHost;
        private readonly IStepDefinitionBuilder _stepDefinitionBuilder;

        public CreateSpecflowStepFromUsageAction(
            SpecflowStepDeclarationReference reference,
            IDialogHost dialogHost,
            IStepDefinitionBuilder stepDefinitionBuilder
        )
        {
            _reference = reference;
            _dialogHost = dialogHost;
            _stepDefinitionBuilder = stepDefinitionBuilder;
        }

        public void Execute(ISolution solution, ITextControl textControl)
        {
            var jetPopupMenus = solution.GetPsiServices().GetComponent<JetPopupMenus>();
            var cache = solution.GetComponent<SpecflowStepsDefinitionsCache>();
            jetPopupMenus.ShowModal(JetPopupMenu.ShowWhen.AutoExecuteIfSingleEnabledItem,
                (lifetime, menu) =>
                {
                    menu.Caption.Value = WindowlessControlAutomation.Create("Where to create the step ?");

                    var availableSteps = cache.GetBindingTypes(_reference.GetElement().GetPsiModule());
                    var filesPerClasses = new OneToSetMap<string, SpecflowStepsDefinitionsCache.AvailableBindingClass>();
                    foreach (var availableBindingClass in availableSteps)
                        filesPerClasses.Add(availableBindingClass.ClassClrName, availableBindingClass);

                    menu.ItemKeys.Add(new Action("Create new binding class", Action.ActionType.CreateBindingClass));
                    menu.ItemKeys.AddRange(filesPerClasses);

                    menu.DescribeItem.Advise(lifetime, e =>
                                                       {
                                                           if (e.Key is Action action)
                                                           {
                                                               e.Descriptor.Icon = CommonThemedIcons.Create.Id;
                                                               e.Descriptor.Style = MenuItemStyle.Enabled;

                                                               e.Descriptor.Text = new RichText(action.Text, DeclaredElementPresenterTextStyles.ParameterInfo.GetStyle(DeclaredElementPresentationPartKind.Type));
                                                               return;
                                                           }

                                                           var (classClrFullName, _) = (KeyValuePair<string, ISet<SpecflowStepsDefinitionsCache.AvailableBindingClass>>) e.Key;

                                                           e.Descriptor.Icon = PsiSymbolsThemedIcons.Class.Id;
                                                           e.Descriptor.Style = MenuItemStyle.Enabled;

                                                           var clrTypeName = new ClrTypeName(classClrFullName);
                                                           e.Descriptor.Text = new RichText(clrTypeName.ShortName, DeclaredElementPresenterTextStyles.ParameterInfo.GetStyle(DeclaredElementPresentationPartKind.Type));
                                                           e.Descriptor.ShortcutText = clrTypeName.GetNamespaceName();
                                                       });
                    menu.ItemClicked.Advise(lifetime, key =>
                                                      {
                                                          if (key is Action action)
                                                          {
                                                              var lifetimeDefinition = new LifetimeDefinition();
                                                              var panel = CreateControl(lifetimeDefinition.Lifetime);
                                                              var dialog = panel.InDialog(
                                                                      "Create new binding class",
                                                                      "",
                                                                      DialogModality.MODAL,
                                                                      BeControlSizes.GetSize(BeControlSizeType.SMALL, BeControlSizeType.SMALL))
                                                                  .WithOkButton(
                                                                      lifetime,
                                                                      () => Console.WriteLine(panel.GetBeControlById<BeTextBox>("className")),
                                                                      disableWhenInvalid: false)
                                                                  .WithCancelButton(lifetimeDefinition.Lifetime);
                                                              _dialogHost.Show(dialog, onDialogDispose:() => {
                                                                  lifetimeDefinition.Terminate();
                                                              });
                                                              return;
                                                          }
                                                          var (_, availableBindingClasses) = (KeyValuePair<string, ISet<SpecflowStepsDefinitionsCache.AvailableBindingClass>>) key;
                                                          OpenFileSelectionModal(jetPopupMenus, textControl, availableBindingClasses, _reference.GetStepKind(), _reference.GetStepText());
                                                      });
                    menu.PopupWindowContextSource = textControl.PopupWindowContextFactory.ForCaret();
                });
        }

        private BeControl CreateControl(Lifetime lifetime)
        {
            var grid = BeControls.GetGrid();
            grid.AddElement("Class name".GetBeLabel());
            grid.AddElement(BeControls.GetTextBox(lifetime, id: "className"));
            return grid;
        }

        private void OpenFileSelectionModal(JetPopupMenus jetPopupMenus, ITextControl textControl, ISet<SpecflowStepsDefinitionsCache.AvailableBindingClass> availableBindingClasses, GherkinStepKind getStepKind, string getStepText)
        {
            jetPopupMenus.ShowModal(JetPopupMenu.ShowWhen.AutoExecuteIfSingleEnabledItem,
                (lifetime, menu) =>
                {
                    menu.Caption.Value = WindowlessControlAutomation.Create("Where to create the step ?");
                    menu.ItemKeys.AddRange(availableBindingClasses);
                    menu.DescribeItem.Advise(lifetime, e =>
                                                       {
                                                           var key = (SpecflowStepsDefinitionsCache.AvailableBindingClass) e.Key;
                                                           e.Descriptor.Icon = PsiCSharpThemedIcons.Csharp.Id;
                                                           e.Descriptor.Style = MenuItemStyle.Enabled;
                                                           e.Descriptor.Text = new RichText(key.SourceFile.DisplayName);
                                                       });
                    menu.ItemClicked.Advise(lifetime, e =>
                                                      {
                                                          var availableBindingClass = (SpecflowStepsDefinitionsCache.AvailableBindingClass) e;
                                                          AddSpecflowStep(availableBindingClass.SourceFile, availableBindingClass.ClassClrName, getStepKind, getStepText);
                                                      });
                    menu.PopupWindowContextSource = textControl.PopupWindowContextFactory.ForCaret();
                });
        }

        private void AddSpecflowStep(IPsiSourceFile targetFile, string classClrName, GherkinStepKind stepKind, string stepText)
        {
            var cSharpFile = targetFile.GetProject().GetCSharpFile(targetFile.DisplayName.Substring(targetFile.DisplayName.LastIndexOf('>') + 2));
            if (cSharpFile == null)
                return;

            foreach (var type in cSharpFile.GetChildrenInSubtrees<IClassDeclaration>())
            {
                if (!(type is IClassDeclaration classDeclaration))
                    continue;
                if (classDeclaration.CLRName != classClrName)
                    continue;
                if (classDeclaration.DeclaredElement?.GetAttributeInstances(AttributesSource.Self).All(x => x.GetAttributeType().GetClrName().FullName != "TechTalk.SpecFlow.Binding") != true)
                    continue;

                var factory = CSharpElementFactory.GetInstance(classDeclaration);
                var methodName = _stepDefinitionBuilder.GetStepDefinitionMethodNameFromStepText(stepKind, stepText, _reference.IsInsideScenarioOutline());
                methodName = cSharpFile.GetPsiServices().Naming.Suggestion.GetDerivedName(methodName, NamedElementKinds.Method, ScopeKind.Common, CSharpLanguage.Instance, new SuggestionOptions(), targetFile);
                var parameters = _stepDefinitionBuilder.GetStepDefinitionParameters(stepText, _reference.IsInsideScenarioOutline());
                var pattern = _stepDefinitionBuilder.GetPattern(stepText, _reference.IsInsideScenarioOutline());

                var attributeType = CSharpTypeFactory.CreateType(SpecflowAttributeHelper.GetAttributeClrName(stepKind), classDeclaration.GetPsiModule());
                var formatString = $"[$0(@\"$1\")] public void {methodName}() {{ScenarioContext.StepIsPending();}}";
                var methodDeclaration = factory.CreateTypeMemberDeclaration(formatString, attributeType, pattern.Replace("\"", "\"\"")) as IMethodDeclaration;
                if (methodDeclaration == null)
                    continue;
                var psiModule = classDeclaration.GetPsiModule();
                foreach (var (parameterName, parameterType) in parameters)
                    methodDeclaration.AddParameterDeclarationAfter(ParameterKind.VALUE, CSharpTypeFactory.CreateType(parameterType, psiModule), parameterName, null);

                IClassMemberDeclaration insertedDeclaration;
                using (new PsiTransactionCookie(type.GetPsiServices(), DefaultAction.Commit, "Generate specflow step"))
                {
                    insertedDeclaration = classDeclaration.AddClassMemberDeclaration((IClassMemberDeclaration) methodDeclaration);
                }

                var invocationExpression = insertedDeclaration.GetChildrenInSubtrees<IInvocationExpression>().FirstOrDefault();
                if (invocationExpression != null)
                    invocationExpression.NavigateToNode(true);
                else
                    insertedDeclaration.NavigateToNode(true);
            }
        }

        private class Action
        {
            public enum ActionType
            {
                CreateBindingClass
            }
            public string Text { get; }
            public ActionType Type { get; }

            public Action(string text, ActionType type)
            {
                Text = text;
                Type = type;
            }
        }
    }
}