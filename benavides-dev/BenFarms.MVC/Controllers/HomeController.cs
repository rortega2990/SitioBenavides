using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Controllers
{
    public class HomeController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public HomeController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var homePage = await GetActiveHomePage();
            if (homePage != null)
                return View(homePage);
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Terreno()
        {
            return View();
        }

        private async Task<HomePage> GetActiveHomePage()
        {
            var h = await storeDB.HomePages.Include(hp => hp.FourQuadSection).Include(hp => hp.DoctorsOfficeSection).FirstOrDefaultAsync(p => p.HomePageActive == true);
            if (h != null)
            {
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.HomePageId && x.ImageSectionPageName == "HomePage").ToListAsync();
                h.BlogSection.Products = h.BlogSection.Products.OrderBy(x => x.ProductOrder).ToList();
                h.OfferSection.Products = h.OfferSection.Products.OrderBy(x => x.ProductOrder).ToList();
                h.OfferTypes = await storeDB.OfferTypes.ToListAsync();
                foreach (var p in h.OfferSection.Products)
                {
                    var offerId = await storeDB.OfferPages.FirstAsync(x => x.OfferPageActive && x.OfferTypeId == p.OfferTypeId);
                    if (offerId != null)
                    {
                        p.ProductURL = "~/Offer/OfferType?id=" + offerId.OfferPageId;
                    }
                }

                foreach (var p in h.BlogSection.Products)
                {
                    var blogId = await storeDB.BlogPages.FirstAsync(x => x.BlogPageActive == "Activada" && x.BlogTypeId == p.BlogTypeId);
                    if (blogId != null)
                    {
                        p.ProductURL = "~/Blog/BlogType?id=" + blogId.BlogTypeId;
                    }
                }

                return h;
            }
            return null;
        }
    }
}