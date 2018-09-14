using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    public class ProviderController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public ProviderController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var providerPage = await GetActiveProviderPage();
            if (providerPage != null)
                return View(providerPage);
            return RedirectToAction("NotFound", "Error");
        }
        
        private async Task<ProviderPage> GetActiveProviderPage()
        {
            var h = storeDB.ProviderPages.FirstOrDefault(p => p.ProviderPageActive);
            if (h != null)
            {
                h.DocumentTypes = await GetActiveDocuments();
                return h;
            }
            return null;
        }

        private async Task<List<DocumentType>> GetActiveDocuments()
        {
            return await storeDB.DocumentTypes.Where(x => x.DocumentActive).ToListAsync();
        }
    }
}