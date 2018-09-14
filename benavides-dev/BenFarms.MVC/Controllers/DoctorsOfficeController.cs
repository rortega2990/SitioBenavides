using BenavidesFarm.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BenFarms.MVC.Controllers
{
    public class DoctorsOfficeController : Controller
    {
        private readonly MyApplicationDbContext context;

        public DoctorsOfficeController()
        {
            context = new MyApplicationDbContext();
        }

        // GET: DoctorsOffice
        public ActionResult Index()
        {
            var activePage = context.DoctorsOfficePages.Where(p => p.Active == true)
                             .Include(p => p.HeadImages)
                             .Include(p => p.ServicesSection)
                             .FirstOrDefault();
            
            return View(activePage);
        }
    }
}