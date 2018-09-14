using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using BenFarms.MVC.Models;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Net;
using System.Linq;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactPageController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ContactPages = db.ContactPages;
            return View(await ContactPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var ContactPage = await db.ContactPages.FindAsync(id);

            if (ContactPage != null)
            {
                var ContactPageActive = await GetActiveContactPage();
                if (ContactPageActive != null)
                {
                    if (!ContactPage.ContactPageActive)
                    {
                        ContactPage.ContactPageActive = true;
                        ContactPageActive.ContactPageActive = false;
                        db.Entry(ContactPage).State = EntityState.Modified;
                        db.Entry(ContactPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.ContactPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> ContactUsersList()
        {
            var ContactUser = db.ContactUsers;
            return View(await ContactUser.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            ContactPage ContactPage;
            if (id == null)
            {
                ContactPage = await GetActiveContactPage();
                if (ContactPage != null)
                    return View(ContactPage);
            }
            ContactPage = await db.ContactPages.FindAsync(id);
            if (ContactPage != null)
            {
                return View(ContactPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewContact = await db.PagePreviews.FindAsync("PreviewContact");

            var str = new MemoryStream(previewContact.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ContactPagePreview;

            var ContactPageActive = await GetActiveContactPage();
            var lastIdContactPage = await db.ContactPages.MaxAsync(x => x.ContactPageId) + 1;

            if (preview == null)
            {
                if (ContactPageActive != null)
                    return View("Preview", ContactPageActive);
            }
            else
            {
                var ContactPagePreview = GetContactPagePreview(ContactPageActive, preview, lastIdContactPage);
                return View("Preview", ContactPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ContactPage", "~/UploadedFiles", "PreviewContact", new ContactPagePreview { ContactPageName = "PreviewContact" }, null, FillDataTextContactPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var ContactPage = await GetActiveContactPage();
            return View(ContactPage);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ContactPage", "~/Content/rsc/imgs", "PreviewContact", new ContactPagePreview { ContactPageName = "PreviewContact" }, null, FillDataTextContactPage, null);

            if (result.Key)
            {
                PagePreview previewContact = await db.PagePreviews.FindAsync("PreviewContact");

            var str = new MemoryStream(previewContact.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ContactPagePreview;

                if (preview != null)
                {
                    var ContactPageActive = await GetActiveContactPage();
                    var lastIdContactPage = await db.ContactPages.MaxAsync(x => x.ContactPageId) + 1;
                    var ContactPageEdit = GetContactPagePreview(ContactPageActive, preview, lastIdContactPage);
                    db.ContactPages.Add(ContactPageEdit);
                    await db.SaveChangesAsync();
                    ContactPageActive.ContactPageActive = false;
                    db.Entry(ContactPageActive).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DetailsContactUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUser contactUser = await db.ContactUsers.FindAsync(id);
            if (contactUser == null)
            {
                return HttpNotFound();
            }
            return View(contactUser);
        }

        public async Task<ActionResult> DeleteContactUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUser contactUser = await db.ContactUsers.FindAsync(id);
            if (contactUser == null)
            {
                return HttpNotFound();
            }
            return View(contactUser);
        }

        [HttpPost, ActionName("DeleteContactUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContactUserConfirmed(int id)
        {
            ContactUser contactUser = await db.ContactUsers.FindAsync(id);
            db.ContactUsers.Remove(contactUser);
            await db.SaveChangesAsync();
            return RedirectToAction("ContactUsersList");
        }

        public ActionResult ConfirmDeletion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var page = db.ContactPages.Where(p => p.ContactPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.ContactPageCreatedDate,
                Id = page.ContactPageId,
                Active = page.ContactPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.ContactPageCustomValue,
                Title = "Eliminar página de contactos"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.ContactPages.Where(p => p.ContactPageId == id).FirstOrDefault();
            if (page != null && page.ContactPageActive == false)
            {
                db.ContactPages.Remove(page);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFound", "Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private ContactPage GetContactPagePreview(ContactPage ContactPageActive, ContactPagePreview preview, int lastIdContactPage)
        {
            var ContactPagePreview = new ContactPage
            {
                ContactPageCustomValue = $"ContactPage_{lastIdContactPage}",
                ContactPageTitle = "Contacto",
                ContactPageActive = true,
                ContactPageCreatedDate = DateTime.Now,
                ContactPageBgColorHead = preview.BgColorHeadContactPage ?? ContactPageActive.ContactPageBgColorHead,
                ContactPageHeadText = preview.HeadTextContactPage ?? ContactPageActive.ContactPageHeadText,
                ContactPageColorHeadText = preview.ColorHeadTextContactPage ?? ContactPageActive.ContactPageColorHeadText,
                ContactPageSubText1 = preview.SubText1ContactPage ?? ContactPageActive.ContactPageSubText1,
                ContactPageSubText2 = preview.SubText2ContactPage ?? ContactPageActive.ContactPageSubText2,
                ContactPageColorSubText1 = preview.ColorSubText1ContactPage ?? ContactPageActive.ContactPageColorSubText1,
                ContactPageColorSubText2 = preview.ColorSubText2ContactPage ?? ContactPageActive.ContactPageColorSubText2,
                ContactPageAddress = preview.AddressContactPage ?? ContactPageActive.ContactPageAddress,
                ContactPageColorTextFooter = preview.ColorTextFooterContactPage ?? ContactPageActive.ContactPageColorTextFooter,
                ContactPageEmailSaD = preview.EmailSaDContactPage ?? ContactPageActive.ContactPageEmailSaD,
                ContactPageTelSaD = preview.TelSaDContactPage ?? ContactPageActive.ContactPageTelSaD,
                ContactPageTelAaP = preview.TelAaPContactPage ?? ContactPageActive.ContactPageTelAaP,
                ContactPageTelAddress = preview.TelAddressContactPage ?? ContactPageActive.ContactPageTelAddress,
            };                        

            return ContactPagePreview;
        }

        private async Task<ContactPage> GetActiveContactPage()
        {
            var h = await db.ContactPages.FirstOrDefaultAsync(p => p.ContactPageActive);
            return h;
        }

        private static void FillDataTextContactPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as ContactPagePreview;
            if (preview != null)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "HeadTextContactPage":
                        preview.HeadTextContactPage = formValue;
                        break;
                    case "ColorHeadTextContactPage":
                        preview.ColorHeadTextContactPage = formValue;
                        break;
                    case "SubText1ContactPage":
                        preview.SubText1ContactPage = formValue;
                        break;
                    case "SubText2ContactPage":
                        preview.SubText2ContactPage = formValue;
                        break;
                    case "AddressContactPage":
                        preview.AddressContactPage = formValue;
                        break;
                    case "BgColorHeadContactPage":
                        preview.BgColorHeadContactPage = formValue;
                        break;
                    case "ColorSubText1ContactPage":
                        preview.ColorSubText1ContactPage = formValue;
                        break;
                    case "ColorSubText2ContactPage":
                        preview.ColorSubText2ContactPage = formValue;
                        break;
                    case "ColorTextFooterContactPage":
                        preview.ColorTextFooterContactPage = formValue;
                        break;
                    case "EmailSaDContactPage":
                        preview.EmailSaDContactPage = formValue;
                        break;
                    case "TelAaPContactPage":
                        preview.TelAaPContactPage = formValue;
                        break;
                    case "TelAddressContactPage":
                        preview.TelAddressContactPage = formValue;
                        break;
                    case "TelSaDContactPage":
                        preview.TelSaDContactPage = formValue;
                        break;
                }
            }
        }
    }
}