using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenFarms.MVC.Controllers
{
    public class ContactController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public ContactController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: Contact
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var ContactPage = await GetActiveContactPage();
            if (ContactPage != null)
                return View(ContactPage);
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendSug(ContactViewModel model)
        {
            var response = new AjaxResponse { Success = false };
            var errorMessages = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ContactUser
                    {
                        Names = model.ContactNames,
                        Email = model.ContactEmail,
                        PhoneNumber = model.ContactPhoneNumber,
                        Suggests = model.ContactSuggests,
                        DateCreation = DateTime.Now
                    };
                    var result = storeDB.ContactUsers.Add(user);
                    await storeDB.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Los datos fueron enviados correctamente";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.Message = "Lo sentimos, no se pudo realizar la operación. Ha ocurrido un error en el servidor.";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                response.Message = "Lo sentimos, no se pudo realizar la operación. Ha ocurrido un error en el servidor.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<ContactPage> GetActiveContactPage()
        {
            var h = await storeDB.ContactPages.FirstOrDefaultAsync(p => p.ContactPageActive);
            return h;
        }
    }
}