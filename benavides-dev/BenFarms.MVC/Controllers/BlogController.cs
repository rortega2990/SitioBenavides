using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Controllers
{
    public class BlogController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public BlogController()
        {            
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> BlogType(int id)
        {
            var blogPage = await GetActiveBlogPage(id);
            if (blogPage != null)
            {
                ViewBag.BlogId = id;
                return View("Index", blogPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> BlogDesc(int id)
        {
            var blogPage = await GetBlogPage(id);
            if (blogPage != null)
                return View("BlogDesc", blogPage);
            return RedirectToAction("NotFound", "Error");
        }

        public async Task<ActionResult> Blog(string type)
        {
            var h = await storeDB.BlogPages.FirstOrDefaultAsync(p => p.BlogPageActive == "Activada" && type.Contains(p.BlogType.BlogTypeName));

            var blogPage = await GetActiveBlogPage(h.BlogTypeId);
            if (blogPage != null)
            {
                ViewBag.BlogId = h.BlogTypeId;
                return View("Index", blogPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> News(int id)
        {
            var newsPage = await GetActiveNewsPage(id);
            if (newsPage != null)
                return View("News", newsPage);
            return RedirectToAction("NotFound", "Error");
        }

       

        private async Task<BlogPage> GetActiveBlogPage(int type)
        {
            var h = await storeDB.BlogPages.FirstOrDefaultAsync(p => p.BlogPageActive == "Activada" && p.BlogTypeId == type);
            h.News = h.News.OrderBy(x => x.NewsPageOrder).ToList();
            foreach(var n in h.News)
            {
                n.NewsPageUrl = "~/Blog/News/" + n.NewsPageId;
            }
            return h;
        }

        private async Task<BlogPage> GetBlogPage(int id)
        {
            var h = await storeDB.BlogPages.FirstOrDefaultAsync(p => p.BlogPageActive == "Activada" && p.BlogTypeId == id);
            h.News = h.News.OrderBy(x => x.NewsPageOrder).ToList();
            foreach (var n in h.News)
            {
                n.NewsPageUrl = "~/Blog/News/" + n.NewsPageId;
            }
            return h;
        }

        private async Task<NewsPage> GetActiveNewsPage(int id)
        {
            var h = await storeDB.NewsPages.FirstOrDefaultAsync(p => p.NewsPageActive == "Activada" && p.NewsPageId == id);
            return h;
        }
    }
}