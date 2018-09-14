using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MailConfigurationController : Controller
    {
        private readonly MyApplicationDbContext context = new MyApplicationDbContext();
        // GET: Admin/InterestRegion
        public async Task<ActionResult> Index()
        {
            return View(await context.MailConfigurations.ToListAsync());
        }

        public ActionResult AddOrEdit(int? id)
        {
            if (id == null)
            {
                return View(new MailConfiguration());
            }

            var target = this.context.MailConfigurations.Find(id);

            if (target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(target);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmAddOrEdit(MailConfiguration model)
        {
            if (ModelState.IsValid == false)
            {
                return View("AddOrEdit", model);
            }

            MailConfiguration target = null;
            if(model.Id > 0)
            {
                target = await context.MailConfigurations.FindAsync(model.Id);
            }
            else
            {
                target = new MailConfiguration();
                target.CreationDate = DateTime.Now;
            }

            if(target == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            target.Server = model.Server;
            target.Port = model.Port;
            target.EnableSSL = model.EnableSSL;
            target.UserName = model.UserName;
            target.Password = model.Password;

            context.MailConfigurations.AddOrUpdate(target);
            await context.SaveChangesAsync();     
           
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> Delete(int? id)
        { 
            MailConfiguration mailConfiguration = await context.MailConfigurations.FindAsync(id);

            if(mailConfiguration == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            context.MailConfigurations.Remove(mailConfiguration);
            await context.SaveChangesAsync();
            //return Json(new AjaxResponse { Success = true, Message = "La Categoría se eliminó correctamente." }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");     
            
        }

        public async Task<ActionResult> SetState(int? id)
        {
            MailConfiguration mailConfiguration = await context.MailConfigurations.FindAsync(id);
            MailConfiguration currentConficuration = await context.MailConfigurations.Where(mc => mc.Active == true).FirstOrDefaultAsync();

           

            if (mailConfiguration == null && currentConficuration == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if(currentConficuration == null && mailConfiguration != null)
            {
                mailConfiguration.Active = true;
                context.Entry(mailConfiguration).State = EntityState.Modified;
            }
            else
            {
                currentConficuration.Active = false;
                mailConfiguration.Active = true;

                context.Entry(currentConficuration).State = EntityState.Modified;
                context.Entry(mailConfiguration).State = EntityState.Modified;
            }           

            await context.SaveChangesAsync();

            return RedirectToAction("Index"); 

        }


        public ActionResult ConfirmDeletion(int? id)
        {
            var target = this.context.MailConfigurations.Find(id);

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
                Title = "Eliminar Configuración de correo"
            };
            return View(model);
        }
    }
}