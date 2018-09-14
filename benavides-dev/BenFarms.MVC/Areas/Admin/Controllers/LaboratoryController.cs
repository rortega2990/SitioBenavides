using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Services;
using BenFarms.MVC.Areas.Admin.Services;
using System.Threading.Tasks;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LaboratoryController : Controller
    {
        private readonly MyApplicationDbContext context;
        public LaboratoryController()
        {
            context = new MyApplicationDbContext();
        }
        // GET: Admin/Laboratory
        public async Task<ActionResult> Index()
        {
            var applicationService = new LaboratoryService(context);
            var laboratories = await applicationService.getLaboratories();
            return View(laboratories);
        }

        [HttpGet]
        public ActionResult AddOrModifyLaboratory(int? id)
        {
            var administrationService = new LaboratoryService(context);
            ViewBag.States = getStatesWithLaboratories(); 
            return View(administrationService.getInputModelForId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate(LaboratoryInputModel inputData)
        {
            var applicationService = new LaboratoryService(context);
            var result = applicationService.addOrUpdateLaboratory(inputData);

            if(result.IsValid)
            {
                return RedirectToAction("Index");
            }

            ViewBag.States = getStatesWithLaboratories();

            ViewBag.Errors = result.Errors;
            return View("AddOrModifyLaboratory", inputData);
        }

       
        public ActionResult Remove(int? id)
        {
            var applicationService = new LaboratoryService(context);
            var operationResult = applicationService.remove(id);
            return RedirectToAction("Index");

        }
        private IEnumerable<SelectListItem> getStatesWithLaboratories()
        {
            var states = context.Estados.Select(st => new SelectListItem()
            {
                Text = st.Name,
                Value = st.Id.ToString()
            }).ToList();

            return states;
        }
    }
}