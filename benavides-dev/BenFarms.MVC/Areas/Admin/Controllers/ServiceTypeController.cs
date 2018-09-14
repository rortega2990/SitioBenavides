using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceTypeController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.ServiceTypes.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceTypeId,ServiceTypeName,ServiceTypeNameDescription,ServiceTypeProdutcsDescription,ServiceTypeActive")] ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                db.ServiceTypes.Add(serviceType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviceType);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceType serviceType = await db.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return HttpNotFound();
            }
            return View(serviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceTypeId,ServiceTypeName,ServiceTypeNameDescription,ServiceTypeProdutcsDescription,ServiceTypeActive")] ServiceType serviceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceType serviceType = await db.ServiceTypes.FindAsync(id);
            if (serviceType == null)
            {
                return HttpNotFound();
            }
            return View(serviceType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceType serviceType = await db.ServiceTypes.FindAsync(id);
            db.ServiceTypes.Remove(serviceType);
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
    }
}
