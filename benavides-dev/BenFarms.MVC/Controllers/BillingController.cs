using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Controllers
{
    public class BillingController : Controller
    {
        private readonly MyApplicationDbContext storeDB;

        public BillingController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            ViewBag.Page = "Billing";
            var billingPage = await GetActiveBillingPage();
            if (billingPage != null)
                return View(billingPage);
            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerPromociones()
        {
            ViewBag.Page = "Billing";
            //var billingPage = await GetActiveBillingPage();
            //if (billingPage != null)
                return View("VerPromociones");
           // return RedirectToAction("NotFound", "Error");
        }


      

        [AllowAnonymous]
        public async Task<ActionResult> PaginaPuente()
        {
            ViewBag.Page = "Billing";
            //var billingPage = await GetActiveBillingPage();
            //if (billingPage != null)
            return View("PaginaPuente");
            // return RedirectToAction("NotFound", "Error");
        }


        private async Task<BillingPage> GetActiveBillingPage()
        {
            var h = await storeDB.BillingPages.FirstOrDefaultAsync(p => p.BillingPageActive);
            if (h != null)
            {
                h.LabSection.ImageSections = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.LabSectionId && x.ImageSectionPageName == "LabSection").ToListAsync();
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.BillingPageId && x.ImageSectionPageName == "BillingPage").ToListAsync();
                return h;
            }
            return null;
        }
    }
}