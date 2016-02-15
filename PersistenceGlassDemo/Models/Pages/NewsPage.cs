using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Shell.Applications.ContentEditor;

namespace PersistenceGlassDemo.Models.Pages
{
    [SitecoreType(true, "{dd26183c-0a0a-427a-9c56-a5f7b3a753b4}", TemplateName = "News page")]
    public class NewsPage : StandardPage
    {
        [SitecoreField("{560d767a-37e6-4a99-bad8-bd7213e67a8c}", SitecoreFieldType.Date, FieldName = "Publication date")]
        public virtual DateTime PublicationDate { get; set; }

        [SitecoreField("{f01c67f1-9e4d-4849-8d93-48d167f12011}", SitecoreFieldType.SingleLineText)]
        public virtual string Headline { get; set; }

        [SitecoreField("{11eb6202-89ed-4e0c-96a0-503dd74680d4}", SitecoreFieldType.RichText, FieldName = "Teaser text")]
        public virtual string TeaserText { get; set; }

        [SitecoreField("{92a03432-b583-452e-a5fa-98f7e605b838}", SitecoreFieldType.RichText, FieldName = "Full news message")]
        public virtual string FullMessage { get; set; }
    }
}