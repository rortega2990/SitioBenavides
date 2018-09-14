using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InterestRegionController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();
        // GET: Admin/InterestRegion
        public async Task<ActionResult> Index()
        {
            return View(await db.InterestRegions.ToListAsync());
        }

        public ActionResult AddOrEdit(int? id)
        {
            if (id == null)
            {
                return View(new InterestRegion());
            }

            var target = this.db.InterestRegions.Find(id);

            if (target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(target);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmAddOrEdit(InterestRegion model)
        {
            if (ModelState.IsValid == false)
            {
                return View("AddOrEdit", model);
            }
            if (model.Id > 0)
            {
                var offe = await db.InterestRegions.FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
                if (offe == null)
                {
                    var interestRegion = await db.InterestRegions.FindAsync(model.Id);
                    if (interestRegion != null)
                    {
                        interestRegion.Name = model.Name;
                        interestRegion.Active = model.Active;
                        interestRegion.MailCollection = model.MailCollection;
                        db.Entry(interestRegion).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                    }
                }
            }
            else
            {
                var region = await db.InterestRegions.FirstOrDefaultAsync(a => a.Name == model.Name);

                if (region == null)
                {
                    region = new InterestRegion()
                    {
                        CreationDate = DateTime.Now,
                        Name = model.Name,
                        Active = model.Active,
                        MailCollection = model.MailCollection
                    };
                    db.InterestRegions.Add(region);
                    await db.SaveChangesAsync();

                }
                else
                {
                    ModelState.AddModelError("", "Ya existe una región con el nombre " + model.Name);
                    return View("AddOrEdit", model);
                }


            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                InterestRegion interestRegion = await db.InterestRegions.FindAsync(id);
                db.InterestRegions.Remove(interestRegion);
                await db.SaveChangesAsync();
                //return Json(new AjaxResponse { Success = true, Message = "La Categoría se eliminó correctamente." }, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //return Json(new AjaxResponse { Success = false, Message = "No se puede eliminar la categoría, está siendo utilizada por otra instancia." }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("NotFound", "Error");
        }


        public ActionResult ConfirmDeletion(int? id)
        {
            var target = this.db.InterestRegions.Find(id);

            if (target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = target.CreationDate,
                Id = target.Id,
                Active = target.Active,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = target.Id.ToString(),
                Title = "Eliminar región"
            };
            return View(model);
        }


    }
}