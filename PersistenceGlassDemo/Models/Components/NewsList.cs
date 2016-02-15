using System;
using BoC.Persistence.SitecoreGlass.Models;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace PersistenceGlassDemo.Models.Components
{
    [SitecoreType(true, "{cc8be8d8-3aed-4513-89df-3123b28d76f5}", TemplateName = "News list")]
    public class NewsList : SitecoreItem
    {
        [SitecoreField("{5332d4ab-1e35-44af-8f91-98b2bb5f222f}", SitecoreFieldType.DropTree, FieldName = "Root item")]
        public virtual Guid NewsRootId { get; set; }

        [SitecoreField("{b5b0daad-0c8b-41a4-ae7b-8b9f5c774dd2}", SitecoreFieldType.Integer, FieldName = "Page size")]
        public virtual int PageSize { get; set; }
    }
}