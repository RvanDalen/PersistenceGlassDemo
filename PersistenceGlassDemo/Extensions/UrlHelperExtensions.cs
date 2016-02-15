using System.Web.Mvc;
using BoC.InversionOfControl;
using BoC.Persistence.SitecoreGlass;
using BoC.Persistence.SitecoreGlass.Models;
using Sitecore.Common;
using Sitecore.Links;

namespace PersistenceGlassDemo.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string SitecoreUrl(this UrlHelper urlHelper, ISitecoreItem sitecoreItem)
        {
            return urlHelper.SitecoreUrl(sitecoreItem, LinkManager.GetDefaultUrlOptions());
        }

        public static string SitecoreUrl(this UrlHelper urlHelper, ISitecoreItem sitecoreItem, UrlOptions urlOptions)
        {
            var db = IoC.Resolver.Resolve<IDatabaseProvider>().GetDatabase();
            return LinkManager.GetItemUrl(db.GetItem(sitecoreItem.Id.ToID()), urlOptions);
        }
    }
}