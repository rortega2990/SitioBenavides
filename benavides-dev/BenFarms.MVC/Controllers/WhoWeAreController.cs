using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    public class WhoWeAreController : Controller
    {
        private readonly MyApplicationDbContext context = new MyApplicationDbContext();
        // GET: WhoWeAre
        public ActionResult Index()
        {
            var activePage = context.WhoWeArePages.Where(p => p.Active == true)
                             .Include(p => p.HeadImages)
                             .Include(p => p.HistoryImages)
                             .Include(p => p.AdSection)
                             .Include(p => p.ValuesSection)
                             .FirstOrDefault();


            if(activePage == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            activePage.HistoryImages.Sort();
            return View(activePage);
        }
    }
}