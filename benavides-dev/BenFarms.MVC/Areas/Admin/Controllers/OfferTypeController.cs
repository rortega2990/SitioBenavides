using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System;
using System.Web;
using BenFarms.MVC.Models;
using System.Linq;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfferTypeController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.OfferTypes.ToListAsync());
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
                    case "NombreTipoOferta":
                        name = formValue;
                        break;
                    case "EstadoTipoOferta":
                        active = formValue == "true";
                        break;
                }
            }
            var offe = await db.OfferTypes.FirstOrDefaultAsync(x => x.OfferTypeName == name);
            if (offe == null)
            {
                var newId = db.OfferTypes.Add(new OfferType { OfferTypeActive = active, OfferTypeName = name });
                await db.SaveChangesAsync();
                return Json(new AjaxResponse { Success = true, Message = "La categoría se insertó correctamente", Data = newId.OfferTypeId }, JsonRequestBehavior.AllowGet);
            }

            return Json(new AjaxResponse { Success = false, Message = "Ya existe una Categoría con ese nombre" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int? id)
        {
            var idOffer = 0;
            var name = "";
            bool active = false;
            var httpRequest = System.Web.HttpContext.Current.Request;
            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "IdTipoOferta":
                        idOffer = int.Parse(formValue);
                        break;
                    case "NombreTipoOferta":
                        name = formValue.Trim();
                        break;
                    case "EstadoTipoOferta":
                        active = formValue == "true";
                        break;
                }                
            }

            var offe = await db.OfferTypes.FirstOrDefaultAsync(x => x.OfferTypeName == name && x.OfferTypeId != idOffer);
            if (offe == null)
            {
                var offerType = await db.OfferTypes.FindAsync(idOffer);
                if (offerType != null)
                {
                    offerType.OfferTypeName = name;
                    offerType.OfferTypeActive = active;
                    db.Entry(offerType).State = EntityState.Modified;
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
                OfferType offerType = await db.OfferTypes.FindAsync(id);
                db.OfferTypes.Remove(offerType);
                await db.SaveChangesAsync();
                return Json(new AjaxResponse { Success = true, Message = "La Categoría se eliminó correctamente." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
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
