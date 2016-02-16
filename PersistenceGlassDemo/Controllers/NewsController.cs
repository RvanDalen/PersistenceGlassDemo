using System.Linq;
using System.Web.Mvc;
using BoC.Persistence;
using PersistenceGlassDemo.Models.Components;
using PersistenceGlassDemo.Models.Pages;

namespace PersistenceGlassDemo.Controllers
{
    public class NewsController : Controller
    {
        private readonly IRepository<NewsPage> _newsRepository;

        public NewsController(IRepository<NewsPage> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public ActionResult Index(NewsList datasource, int currentPage = 1)
        {
            var newsItems = _newsRepository.Query()
                                           .Where(news => news.ParentIds.Contains(datasource.NewsRootId))
                                           .OrderByDescending(news => news.PublicationDate) //If you uncomment the NewsRepositories class then you can comment out or remove this line
                                           .Skip(currentPage - 1)
                                           .Take(datasource.PageSize)
                                           .ToList();
            
            return View(newsItems);
        }

        public ActionResult Detail(NewsPage contextItem)
        {
            return View(contextItem);
        }
    }
}   