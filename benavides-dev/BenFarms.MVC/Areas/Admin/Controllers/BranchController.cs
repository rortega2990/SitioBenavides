using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Services;
using BenFarms.MVC.Areas.Admin.Models;
using System.Linq;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Branchs.Include(b => b.State).Include(b => b.City).ToListAsync());
        }

        public ActionResult Create()
        {
            ViewBag.States = db.Estados.Select(st => new SelectListItem() { Text = st.Name, Value = st.Id.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BranchInputModel inputData)
        {
            var service = new BranchAdministrationApplicationService(db);
            var operationResult = await service.create(inputData);

            if (operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }
            AddErrorsToModelState(operationResult);
            ViewBag.States = db.Estados.Select(st => new SelectListItem() { Text = st.Name, Value = st.Id.ToString() }).ToList();
            return View(inputData);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = await db.Branchs.Where(b => b.BranchId == id.Value).Include(b => b.State).Include(b => b.City).FirstOrDefaultAsync();
            if (branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.States = db.Estados.Select(st => new SelectListItem() { Text = st.Name, Value = st.Id.ToString() }).ToList();

            return View(new BranchInputModel {
                BranchName = branch.BranchName,
                BranchActive = branch.BranchActive,
                BranchAddress = branch.BranchAddress,
                BranchCeco = branch.BranchCeco,
                BranchCity = branch.BranchCity,
                City = (branch.City == null) ? 0 : branch.City.Id,
                State = (branch.State == null) ? 0 :branch.State.Id,
                BranchConsult = branch.BranchConsult,
                BranchHour1 = branch.BranchHour1,
                BranchHour2 = branch.BranchHour2,
                BranchId = branch.BranchId,
                BranchLatitude = branch.BranchLatitude,
                BranchLongitude = branch.BranchLongitude,
                BranchRegion = branch.BranchRegion,
                BranchSap = branch.BranchSap,
                BranchFose = branch.BranchFose,
                BranchTwentyFourHours = branch.BranchTwentyFourHours
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BranchInputModel branch)
        {
            var service = new BranchAdministrationApplicationService(db);
            var operationResult = await service.create(branch);
            if(operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }

            AddErrorsToModelState(operationResult);
            ViewBag.States = db.Estados.Select(st => new SelectListItem() { Text = st.Name, Value = st.Id.ToString() }).ToList();
            return View(branch);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = await db.Branchs.FindAsync(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Branch branch = await db.Branchs.FindAsync(id);
            db.Branchs.Remove(branch);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
   
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AddErrorsToModelState(AdministrationServiceResult result)
        {
            foreach(string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}
