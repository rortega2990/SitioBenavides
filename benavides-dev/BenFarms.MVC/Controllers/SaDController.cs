using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    public class SaDController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public SaDController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: SaD
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var SaDPage = await GetActiveSaDPage();
            if (SaDPage != null)
                return View(SaDPage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<SaDPage> GetActiveSaDPage()
        {
            var h = await storeDB.SaDPages.FirstOrDefaultAsync(p => p.SaDPageActive);
            if (h != null)
            {
                h.SaDTypeNumbers = await GetActiveSaDTypess();
                return h;
            }
            return null;
        }

        private async Task<List<SaDTypeNumber>> GetActiveSaDTypess()
        {

            List<SaDTypeNumber> result = await storeDB.SaDTypeNumbers.Where(x => x.SaDTypeNumberActive).OrderBy( s=> s.SaDTypeNumberCity).ToListAsync();

            //result.Sort();

            return result;
        }
    }
}