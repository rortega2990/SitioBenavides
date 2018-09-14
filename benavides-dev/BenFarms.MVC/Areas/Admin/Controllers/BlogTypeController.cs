using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogTypeController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.BlogTypes.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Create(int? id)
        {
            var name = "";
            bool active = false;
            var httpRequest = System.Web.HttpContext.Current.Request;
            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "NombreTipoBlog":
                        name = formValue;
                        break;
                    case "EstadoTipoBlog":
                        active = formValue == "true";
                        break;
                }
            }
            var offe = await db.BlogTypes.FirstOrDefaultAsync(x => x.BlogTypeName == name);
            if (offe == null)
            {
                var newId = db.BlogTypes.Add(new BlogType { BlogTypeActive = active, BlogTypeName = name });
                await db.SaveChangesAsync();
                return Json(new AjaxResponse { Success = true, Message = "La categoría se insertó correctamente", Data = newId.BlogTypeId }, JsonRequestBehavior.AllowGet);
            }

            return Json(new AjaxResponse { Success = false, Message = "Ya existe una Categoría con ese nombre" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int? id)
        {
            var idBlog = 0;
            var name = "";
            bool active = false;
            var httpRequest = System.Web.HttpContext.Current.Request;
            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "IdTipoBlog":
                        idBlog = int.Parse(formValue);
                        break;
                    case "NombreTipoBlog":
                        name = formValue.Trim();
                        break;
                    case "EstadoTipoBlog":
                        active = formValue == "true";
                        break;
                }
            }

            var offe = await db.BlogTypes.FirstOrDefaultAsync(x => x.BlogTypeName == name && x.BlogTypeId != idBlog);
            if (offe == null)
            {
                var blogType = await db.BlogTypes.FindAsync(idBlog);
                if (blogType != null)
                {
                    blogType.BlogTypeName = name;
                    blogType.BlogTypeActive = active;
                    db.Entry(blogType).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "La Categoría se actualizó correctamente" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new AjaxResponse { Success = false, Message = "Ya existe una Categoría con ese nombre" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                BlogType blogType = await db.BlogTypes.FindAsync(id);
                db.BlogTypes.Remove(blogType);
                await db.SaveChangesAsync();
                return Json(new AjaxResponse { Success = true, Message = "La Categoría se eliminó correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "No se puede eliminar la categoría, está siendo utilizada por otra instancia." }, JsonRequestBehavior.AllowGet);
            }
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