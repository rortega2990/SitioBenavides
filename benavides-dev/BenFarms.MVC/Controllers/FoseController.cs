using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Controllers
{
    public class FoseController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public FoseController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var FosePage = await GetActiveFosePage();
            if (FosePage != null)
                return View(FosePage);
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Products(int id)
        {
            var promocionPage = await GetActiveProducts(id);
            if (promocionPage != null)
                return View(promocionPage);
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ImageProduct(int id)
        {
            var imageProductPage = await GetActiveImageProduct(id);
            if (imageProductPage != null)
            {
                ViewBag.IdPromocion = imageProductPage.PromocionPageId;
                return View(imageProductPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<FosePage> GetActiveFosePage()
        {
            var h = await storeDB.FosePages.FirstOrDefaultAsync(p => p.FosePageActive == "Activada");
            if (h != null)
            {
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.FosePageId && x.ImageSectionPageName == "FosePage").ToListAsync();
                h.Promocions = h.Promocions.OrderBy(x => x.PromocionPageOrder).ToList();

                //foreach (var y in h.Promocions)
                //{
                //    y.HeadImage = await storeDB.ImageSections.FirstOrDefaultAsync(x => x.ImageSectionPageId == y.PromocionPageId && x.ImageSectionPageName == "FosePromocion");
                //}

                return h;
            }
            return null;
        }

        private async Task<PromocionPage> GetActiveProducts(int id)
        {
            var h = await storeDB.PromocionPages.FirstOrDefaultAsync(p => p.PromocionPageId == id);
            if (h != null)
            {
                h.ProductPages = await storeDB.ProductPages.Where(p => p.PromocionPageId == id).OrderBy(x => x.Product.ProductOrder).ToListAsync();
                return h;
            }
            return null;
        }

        private async Task<ProductPage> GetActiveImageProduct(int id)
        {
            var h = await storeDB.ProductPages.FirstOrDefaultAsync(p => p.ProductPageId == id);
            return h;
        }
    }
}