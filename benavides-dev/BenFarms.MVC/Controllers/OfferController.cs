using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Controllers
{
    public class OfferController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public OfferController()
        {            
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> OfferType(int id)
        {
            var offerPage = await GetActiveOfferPage(id);
            if (offerPage != null)
                return View("Index", offerPage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<OfferPage> GetActiveOfferPage(int type)
        {
            var h = await storeDB.OfferPages.FirstOrDefaultAsync(p => p.OfferPageActive && p.OfferPageId == type);
            return h;
        }
    }
}