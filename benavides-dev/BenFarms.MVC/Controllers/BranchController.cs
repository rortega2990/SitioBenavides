using System.Collections.Generic;
using System.Data.Entity;
using BenavidesFarm.DataModels.Models.Pages;
using System.Linq;
using System.Threading.Tasks;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using BenFarms.MVC.Models;
using BenFarms.MVC.Services;

namespace BenFarms.MVC.Controllers
{
    public class BranchController : Controller
    {

        readonly MyApplicationDbContext storeDB;

        public BranchController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var branchPage = await GetActiveBranchPage();
            var estados = await GetStates();

            ViewBag.Estados = estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name });

            if (branchPage != null)
                return View(branchPage);

            return RedirectToAction("NotFound", "Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Search( BranchSearchInputModel searchCriteria)
        {
            var branchPage = await GetActiveBranchPage();

            if(ModelState.IsValid == false)
            {
                int a = 0;
            }

            if (branchPage != null)
            {
                BranchesApplicationService service = new BranchesApplicationService(storeDB);
                BranchSearchResultViewModel result = service.getBranchesByLocaltionriteria(searchCriteria);
                ViewBag.SearchResults = result;
                return View(branchPage);
            }

            return RedirectToAction("NotFound", "Error");

        }
        private async Task<BranchPage> GetActiveBranchPage()
        {
            var h = await storeDB.BranchPages.FirstOrDefaultAsync(p => p.BranchPageActive);
            if (h != null)
            {
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.BranchPageId && x.ImageSectionPageName == "BranchPage").ToListAsync();
                h.Branchs = await GetActiveBranchs24();
                return h;
            }
            return null;
        }

        private async Task<List<Branch>> GetActiveBranchs24()
        {
            return await storeDB.Branchs.Where(x => x.BranchActive && x.BranchConsult).ToListAsync();
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var response = new AjaxResponse { Success = false };
            var branchs = await GetActiveBranchs();
            //return Request.CreateResponse(HttpStatusCode.OK, branchs);
            response.Message = "Success";
            response.Data = branchs;
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> GetCities(int? id)
        {
            var cities = await GetCitiesFromState(id);
            return Json(cities.Select(e => new { Value = e.Id.ToString(), Text = e.Name }), JsonRequestBehavior.AllowGet);
        }
        private async Task<List<Branch>> GetActiveBranchs()
        {
            return await storeDB.Branchs.Where(x => x.BranchActive).ToListAsync();
        }
        private async Task<List<Estados>> GetStates()
        {
            return await storeDB.Estados.OrderBy(st => st.Name).ToListAsync();
        }
        private async Task<List<Municipios>> GetCitiesFromState(int? id)
        {
            if(id != null)
            {
                return await storeDB.EstadosMunicipios.Where(em => em.Estado.Id == id).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();
            }
            return new List<Municipios>();
        }

    }
}