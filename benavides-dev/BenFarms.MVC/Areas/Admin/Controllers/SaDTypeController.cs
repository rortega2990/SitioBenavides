using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaDTypeController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.SaDTypeNumbers.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaDTypeNumberId,SaDTypeNumberCity,SaDTypeNumberPhone,SaDTypeNumberActive")] SaDTypeNumber saDTypeNumber)
        {
            if (ModelState.IsValid)
            {
                db.SaDTypeNumbers.Add(saDTypeNumber);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(saDTypeNumber);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaDTypeNumber saDTypeNumber = await db.SaDTypeNumbers.FindAsync(id);
            if (saDTypeNumber == null)
            {
                return HttpNotFound();
            }
            return View(saDTypeNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaDTypeNumberId,SaDTypeNumberCity,SaDTypeNumberPhone,SaDTypeNumberActive")] SaDTypeNumber saDTypeNumber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saDTypeNumber).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(saDTypeNumber);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaDTypeNumber saDTypeNumber = await db.SaDTypeNumbers.FindAsync(id);
            if (saDTypeNumber == null)
            {
                return HttpNotFound();
            }
            return View(saDTypeNumber);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SaDTypeNumber saDTypeNumber = await db.SaDTypeNumbers.FindAsync(id);
            db.SaDTypeNumbers.Remove(saDTypeNumber);
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
