using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System;
using BenFarms.MVC.Models;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InterestAreaController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.InterestAreas.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Create(int? id)
        {
            /*var name = "";
            bool active = false;
            var httpRequest = System.Web.HttpContext.Current.Request;
            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "NombreTipoArea":
                        name = formValue;
                        break;
                    case "EstadoTipoArea":
                        active = formValue == "true";
                        break;
                }
            }
            var offe = await db.InterestAreas.FirstOrDefaultAsync(x => x.InterestAreaName == name);
            if (offe == null)
            {
                var newId = db.InterestAreas.Add(new InterestArea { InterestAreaActive = active, CreationDate = DateTime.Now, InterestAreaName = name });
                await db.SaveChangesAsync();
                return Json(new AjaxResponse { Success = true, Message = "La categoría se insertó correctamente", Data = newId.InterestAreaId }, JsonRequestBehavior.AllowGet);
            }*/

            return Json(new AjaxResponse { Success = false, Message = "Ya existe una Categoría con ese nombre" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrEdit(int? id)
        {
            if(id == null)
            {
                return View(new InterestArea());
            }

            var target = this.db.InterestAreas.Find(id);

            if (target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(target);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmAddOrEdit(InterestArea model)
        {
            if (ModelState.IsValid == false)
            {
                return View("AddOrEdit", model);
            }
            if(model.InterestAreaId > 0)
            {
                var offe = await db.InterestAreas.FirstOrDefaultAsync(x => x.InterestAreaName == model.InterestAreaName && x.InterestAreaId != model.InterestAreaId);
                if (offe == null)
                {
                    var interestArea = await db.InterestAreas.FindAsync(model.InterestAreaId);
                    if (interestArea != null)
                    {
                        interestArea.InterestAreaName = model.InterestAreaName;
                        interestArea.InterestAreaActive = model.InterestAreaActive;
                        interestArea.MailCollection = model.MailCollection;
                        db.Entry(interestArea).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                            
                    }
                }
            }
            else
            {
                var area = await db.InterestAreas.FirstOrDefaultAsync(a => a.InterestAreaName == model.InterestAreaName);

                if(area == null)
                {
                    area = new InterestArea()
                    {
                        CreationDate = DateTime.Now,
                        InterestAreaName = model.InterestAreaName,
                        InterestAreaActive = model.InterestAreaActive,
                        MailCollection = model.MailCollection
                    };
                    db.InterestAreas.Add(area);
                    await db.SaveChangesAsync();
                        
                }
                else
                {
                    ModelState.AddModelError("", "Ya existe un área con el nombre " + model.InterestAreaName);
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
                InterestArea interestArea = await db.InterestAreas.FindAsync(id);
                db.InterestAreas.Remove(interestArea);
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
            var target = this.db.InterestAreas.Find(id);

            if(target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
           
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = target.CreationDate,
                Id = target.InterestAreaId,
                Active = target.InterestAreaActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue =target.InterestAreaId.ToString(),
                Title = "Eliminar área"
            };
            return View(model);
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