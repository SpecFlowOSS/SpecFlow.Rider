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
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
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

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
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
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
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
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinTagClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.TAG;

        public GherkinTagClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0xBB, 0xB5, 0x29);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinParameterClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.REGEXP_PARAMETER;

        public GherkinParameterClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0xEE, 0x82, 0xEE);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinTableCellClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.TABLE_CELL;

        public GherkinTableCellClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0x29, 0x7B, 0xDE);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinOutlineParameterClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.OUTLINE_PARAMETER_SUBSTITUTION;

        public GherkinOutlineParameterClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0x56, 0x9C, 0xD6);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinTableHeaderClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.TABLE_HEADER_CELL;

        public GherkinTableHeaderClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0xEE, 0x82, 0xEE);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinPipeClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.PIPE;

        public GherkinPipeClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0x56, 0x9C, 0xD6);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
    
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = VsSyntaxPriorityClassificationDefinition.Name, Before = VsAnalysisPriorityClassificationDefinition.Name)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(Name)]
    [System.ComponentModel.DisplayName(Name)]
    [UserVisible(true)]
    internal class GherkinPystringClassificationDefinition : ClassificationFormatDefinition
    {
        private const string Name = GherkinHighlightingAttributeIds.PYSTRING;

        public GherkinPystringClassificationDefinition()
        {
            DisplayName = Name;
            ForegroundColor = Color.FromRgb(0xD6, 0x9D, 0x85);
        }

        [Export, Name(Name), BaseDefinition("formal language")]
        internal ClassificationTypeDefinition ClassificationTypeDefinition;
    }
}

#endif