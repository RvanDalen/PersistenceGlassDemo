using BoC.Persistence.SitecoreGlass.Models;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace PersistenceGlassDemo.Models.Pages
{
    [SitecoreType(true, "{a6476f4b-082b-423e-afbb-c4e10437af06}", TemplateName = "Standard page")]
    public class StandardPage : SitecoreItem
    {
    }
}
