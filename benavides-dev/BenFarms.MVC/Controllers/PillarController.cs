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
    public class PillarController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public PillarController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: Pillar
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var pillarPage = await GetActivePillarPage();
            if (pillarPage != null)
                return View(pillarPage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<PillarPage> GetActivePillarPage()
        {
            var h = await storeDB.PillarPages.FirstOrDefaultAsync(p => p.PillarPageActive);
            if (h != null)
            {
                h.Pillars = await GetActivePillars();
                h.Quotes = await GetQuotes();
                return h;
            }
            return null;
        }

        private async Task<List<Pillar>> GetActivePillars()
        {
            return await storeDB.Pillars.Where(x => x.PillarActive).ToListAsync();
        }

        private async Task<List<Quote>> GetQuotes()
        {
            return await storeDB.Quotes.ToListAsync();
        }
    }
}