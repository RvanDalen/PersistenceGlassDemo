using System;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;

namespace PersistenceGlassDemo.Pipelines.GetChromeData
{
    /// <summary>
    /// https://jeffdarchuk.wordpress.com/2015/12/20/super-field-editor/
    /// </summary>
    public class SuperFieldEditor : GetChromeDataProcessor
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if (!"rendering".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            //We need to ensure that the cache is fully populated
            args.Item.Fields.ReadAll();
            var fields = string.Join("|", args.Item.Fields.Where(f => !f.Name.StartsWith("__")).Select(f => f.Name));
            AddButtonsToChromeData(new[] {
                new WebEditButton
                {
                    Click = string.Format("webedit:fieldeditor(fields={0}, command={{70C4EED5-D4CD-4D7D-9763-80C42504F5E7}})", fields),
                    Icon = args.Item.Appearance.Icon,
                    Tooltip = "Edit the fields for "+args.Item.Name,
                    Header = "Edit fields",
                    Type = "sticky" // sticky keeps it from being hidden in the 'more' dropdown
                }
            }
            , args);
        }
    }
}
