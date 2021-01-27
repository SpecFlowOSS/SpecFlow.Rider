using System.ComponentModel.Composition;
using System.Windows.Media;
using JetBrains.Platform.VisualStudio.SinceVs10.TextControl.Markup.FormatDefinitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

#if !RIDER

// NOTE: This file is explicitly excluded by the .csproj for Mac/Linux!

// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global

// Field is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.ReSharper
{
    internal static class ClassificationTypes
    {
        [Export, Name(GherkinHighlightingAttributeIds.KEYWORD), BaseDefinition("formal language")]
        internal static ClassificationTypeDefinition ClassificationTypeDefinition;
    }

    internal static class ClassificationFormats
    {
        [ClassificationType(ClassificationTypeNames = Name)]
        [Order(After = VsSyntaxPriorityClassificationDefinition.Name,
            Before = VsAnalysisPriorityClassificationDefinition.Name)]
        [Export(typeof(EditorFormatDefinition))]
        [Name(Name)]
        [System.ComponentModel.DisplayName(Name)]
        [UserVisible(true)]
        internal class GherkinKeywordClassificationDefinition : ClassificationFormatDefinition
        {
            private const string Name = GherkinHighlightingAttributeIds.KEYWORD;

            [ImportingConstructor]
            public GherkinKeywordClassificationDefinition()
            {
                DisplayName = Name;
                ForegroundColor = Color.FromRgb(0x56, 0x9C, 0xD6);
            }

//            [Export, Name(Name), BaseDefinition("formal language")]
//            internal ClassificationTypeDefinition ClassificationTypeDefinition;
        }

        [ClassificationType(ClassificationTypeNames = Name)]
        [Order(After = VsSyntaxPriorityClassificationDefinition.Name,
            Before = VsAnalysisPriorityClassificationDefinition.Name)]
        [Export(typeof(EditorFormatDefinition))]
        [Name(Name)]
        [System.ComponentModel.DisplayName(Name)]
        [UserVisible(true)]
        internal class GherkinTextClassificationDefinition : ClassificationFormatDefinition
        {
            private const string Name = GherkinHighlightingAttributeIds.TEXT;

            public GherkinTextClassificationDefinition()
            {
                DisplayName = Name;
                ForegroundColor = Color.FromRgb(0xDC, 0xDC, 0xDC);
            }

            [Export, Name(Name), BaseDefinition("formal language")]
            internal ClassificationTypeDefinition ClassificationTypeDefinition;
        }

        [ClassificationType(ClassificationTypeNames = Name)]
        [Order(After = VsSyntaxPriorityClassificationDefinition.Name,
            Before = VsAnalysisPriorityClassificationDefinition.Name)]
        [Export(typeof(EditorFormatDefinition))]
        [Name(Name)]
        [System.ComponentModel.DisplayName(Name)]
        [UserVisible(true)]
        internal class GherkinLineCommentClassificationDefinition : ClassificationFormatDefinition
        {
            private const string Name = GherkinHighlightingAttributeIds.LINE_COMMENT;

            public GherkinLineCommentClassificationDefinition()
            {
                DisplayName = Name;
                ForegroundColor = Color.FromRgb(0x57, 0xA6, 0x4A);
            }

            [Export, Name(Name), BaseDefinition("formal language")]
            internal ClassificationTypeDefinition ClassificationTypeDefinition;
        }

//    [ClassificationType(ClassificationTypeNames = Name)]
//    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
//    [Export(typeof(EditorFormatDefinition))]
//    [Name(Name)]
//    [System.ComponentModel.DisplayName(Name)]
//    [UserVisible(true)]
//    internal class GherkinTagClassificationDefinition : ClassificationFormatDefinition
//    {
//        private const string Name = GherkinHighlightingAttributeIds.TAG;
//
//        public GherkinTagClassificationDefinition()
//        {
//            DisplayName = Name;
//            ForegroundColor = Color.FromRgb(0xBB, 0xB5, 0x29);
//        }
//
//        [Export, Name(Name), BaseDefinition("formal language")]
//        internal ClassificationTypeDefinition ClassificationTypeDefinition;
//    }
    }
}

#endif