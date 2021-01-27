using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using JetBrains.Application;
using Microsoft.VisualStudio.Text.Classification;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.ReSharper
{
    [ShellComponent]
    public class TestShellComponent
    {
        [ImportMany]
        internal List<Lazy<ClassificationTypeDefinition>> _classificationTypeDefinitions { get; set; }

        public TestShellComponent(Lazy<IClassificationTypeRegistryService> classificationRegistry)
        {
            var value = classificationRegistry.Value;
            var type = value.GetClassificationType("ReSharper SpecFlow Keyword");
            Console.WriteLine(type);

            Task.Delay(5000).ContinueWith(o => TestMethod());
        }

        private void TestMethod()
        {
            Console.WriteLine(_classificationTypeDefinitions);
        }
    }
}