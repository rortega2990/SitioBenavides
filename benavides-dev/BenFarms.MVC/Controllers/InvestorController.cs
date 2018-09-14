using System;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Threading.Tasks;
using System.Data.Entity;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Controllers
{
    public class InvestorController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public InvestorController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: Investor
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var investorPage = await GetActiveInvestorPage();
            if (investorPage != null)
                return View(investorPage);
            return RedirectToAction("NotFound", "Error");
        }

        private async Task<InvestorPage> GetActiveInvestorPage()
        {
            var h = await storeDB.InvestorPages.FirstOrDefaultAsync(p => p.InvestorPageActive);
            if (h != null)
            {
                h.ReportTypes = await GetActiveReports();
                return h;
            }
            return null;
        }

        private async Task<List<ReportType>> GetActiveReports()
        {
            return await storeDB.ReportTypes.Where(x => x.ReportActive).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult> FilesByIdYear(int idReport, int year)
        {
            try
            {
                var files = await storeDB.ReportFiles.Where(x => x.Year == year && x.ReportTypeId == idReport).ToListAsync();

                foreach (var f in files)
                {
                    f.AddressFile = f.AddressFile.Substring(1);
                }
                return Json(new AjaxResponse { Success = true, Message = "Success", Data = files }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "Ocurrión un error interno en el servidor" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}