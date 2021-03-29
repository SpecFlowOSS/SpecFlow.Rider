using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    public class FeatureFileTemplateSectionMarker : Everywhere, IMainScopePoint
    {
        private static readonly Guid ourDefaultUID = new Guid("03C3FCCF-1B7E-4A4A-8D1B-581A1F2661F1");
        private static readonly Guid QuickUID = new Guid("C7551EB2-BBED-446C-8C4A-6BEF43963231");


        
        public override Guid GetDefaultUID() => ourDefaultUID;

        public override string PresentableShortName => "Feature file template";
       

        public string QuickListTitle => "SpecFlow files";
        public Guid QuickListUID => QuickUID;
    }
}