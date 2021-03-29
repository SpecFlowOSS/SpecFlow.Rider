using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Application.Settings.Implementation;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.Util;

namespace ReSharperPlugin.SpecflowRiderPlugin.Templates
{
    [ShellComponent]
    public class SpecFlowQuickListDefaultSettings : HaveDefaultSettings
    {
        private readonly ILogger myLogger;
        private readonly ISettingsSchema mySettingsSchema;
        private readonly IMainScopePoint myProjectMainPoint;
        // private readonly IMainScopePoint myFilesMainPoint;


        public SpecFlowQuickListDefaultSettings(ILogger logger, ISettingsSchema settingsSchema, SpecFlowProjectScopeCategoryUIProvider projectScopeProvider) : base(logger, settingsSchema)
        {
            myLogger = logger;
            mySettingsSchema = settingsSchema;
            myProjectMainPoint = projectScopeProvider.MainPoint;
        }

        public override void InitDefaultSettings(ISettingsStorageMountPoint mountPoint)
        {
            InitialiseQuickList(mountPoint, myProjectMainPoint);
            // InitialiseQuickList(mountPoint, myFilesMainPoint);
            
            var pos = 0;
            AddToQuickList(mountPoint, myProjectMainPoint, "Gherkin", ++pos, "999275DA-907D-4422-854B-91BA0C2157E7");
        }

        private void AddToQuickList(ISettingsStorageMountPoint mountPoint, IMainScopePoint quickList, string name, int position, string guid)
        {
            var quickListKey = mySettingsSchema.GetIndexedKey<QuickListSettings>();
            var entryKey = mySettingsSchema.GetIndexedKey<EntrySettings>();
            var dictionary = new Dictionary<SettingsKey, object>
            {
                {quickListKey, new GuidIndex(quickList.QuickListUID)},
                {entryKey, new GuidIndex(new Guid(guid))}
            };

            if (!ScalarSettingsStoreAccess.IsIndexedKeyDefined(mountPoint, entryKey, dictionary, null, myLogger))
                ScalarSettingsStoreAccess.CreateIndexedKey(mountPoint, entryKey, dictionary, null, myLogger);
            SetValue(mountPoint, (EntrySettings e) => e.EntryName, name, dictionary);
            SetValue(mountPoint, (EntrySettings e) => e.Position, position, dictionary);
        }

        private void InitialiseQuickList(ISettingsStorageMountPoint mountPoint, IMainScopePoint quickList)
        {
            var settings = new QuickListSettings {Name = quickList.QuickListTitle};
            SetIndexedKey(mountPoint, settings, new GuidIndex(quickList.QuickListUID));
        }

        public override string Name => "SpecFlow QuickList settings";
    }
}