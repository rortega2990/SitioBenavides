using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    public class ServiceController : Controller
    {   
        readonly MyApplicationDbContext storeDB;

        public ServiceController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: Service
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var servicePage = await GetActiveServicePage();
            if (servicePage != null)
                return View(servicePage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<ServicePage> GetActiveServicePage()
        {
            var h = await storeDB.ServicePages.FirstOrDefaultAsync(p => p.ServicePageActive);
            if (h != null)
            {
                h.ServiceTypes = await GetActiveServiceTypes();
                return h;
            }
            return null;
        }

        private async Task<List<ServiceType>> GetActiveServiceTypes()
        {
            return await storeDB.ServiceTypes.Where(x => x.ServiceTypeActive).ToListAsync();
        }
    }
}