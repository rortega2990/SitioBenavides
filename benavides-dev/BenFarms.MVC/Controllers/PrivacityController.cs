using System.Linq;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    public class PrivacityController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public PrivacityController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: Privacity
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var providerPage = await GetActivePrivacityPage();
            if (providerPage != null)
                return View(providerPage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<PrivacityPage> GetActivePrivacityPage()
        {
            var h = await storeDB.PrivacityPages.FirstOrDefaultAsync(p => p.PrivacityPageActive);
            return h;
        }
    }
}