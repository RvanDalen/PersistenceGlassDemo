using System.Linq;
using BoC.Logging;
using BoC.Persistence;
using BoC.Persistence.SitecoreGlass;
using BoC.Profiling;
using PersistenceGlassDemo.Models.Pages;
using Sitecore.ContentSearch;

namespace PersistenceGlassDemo.Repositories
{
    /// <summary>
    /// My own implementation of IRepository which will prevent a generated repository getting registered for NewsPage.
    /// Uncomment to start using this repo. It removes the need for sorting on publication date in the NewsController.
    /// </summary>
    //public class NewsRepository : SitecoreRepository<NewsPage>, IRepository<NewsPage>
    //{
    //    private readonly IProviderSearchContextProvider _providerSearchContextProvider;
    //    private readonly IDatabaseProvider _databaseProvider;

    //    public NewsRepository(IDatabaseProvider dbProvider, ISitecoreServiceProvider sitecoreServiceProvider, IProviderSearchContextProvider searchContextProvider, ILogger logger) : base(dbProvider, sitecoreServiceProvider, searchContextProvider, logger)
    //    {
    //        _databaseProvider = dbProvider;
    //        _providerSearchContextProvider = searchContextProvider;
    //    }

    //    public IQueryable<NewsPage> Query()
    //    {
    //        using (Profiler.StartContext("NewsRepository.Query()"))
    //        {
    //            var query = _providerSearchContextProvider.GetProviderSearchContext()
    //                                                      .GetQueryable<NewsPage>(new CultureExecutionContext(GetLanguage(_databaseProvider).CultureInfo));
    //            query = AddStandardQueries(query);
    //            return AddStandardSorting(query);
    //        }
    //    }

    //    private IQueryable<NewsPage> AddStandardSorting(IQueryable<NewsPage> query)
    //    {
    //        return query.OrderByDescending(news => news.PublicationDate);
    //    }
    //}
}