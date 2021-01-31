using System.Collections.Generic;
using System.Drawing;
using JetBrains.Application.UI.Controls.BulbMenu.Items;
using JetBrains.Application.UI.Icons.ColorIcons;
using JetBrains.TextControl.DocumentMarkup;
using JetBrains.Util;

namespace ReSharperPlugin.SpecflowRiderPlugin.Daemon.ExecutionFailedStep
{
    public class ExecutionFailedStepGutterMarkType : IconGutterMarkType
    {
        public ExecutionFailedStepGutterMarkType() : base(new ColorIconId(Color.Crimson))
        {
        }

        public override IEnumerable<BulbMenuItem> GetBulbMenuItems(IHighlighter highlighter)
        {
            return EmptyList<BulbMenuItem>.Enumerable;
        }
    }
}