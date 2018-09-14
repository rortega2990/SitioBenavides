using System;
using System.Data.Entity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Models;
using System.Web;
using System.Linq;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConditionsTermsPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ConditionsTermsPages = db.ConditionsTermsPages;
            return View(await ConditionsTermsPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var ConditionsTermsPage = await db.ConditionsTermsPages.FindAsync(id);

            if (ConditionsTermsPage != null)
            {
                var ConditionsTermsPageActive = await GetActiveConditionsTermsPage();
                if (ConditionsTermsPageActive != null)
                {
                    if (!ConditionsTermsPage.ConditionsTermsPageActive)
                    {
                        ConditionsTermsPage.ConditionsTermsPageActive = true;
                        ConditionsTermsPageActive.ConditionsTermsPageActive = false;
                        db.Entry(ConditionsTermsPage).State = EntityState.Modified;
                        db.Entry(ConditionsTermsPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.ConditionsTermsPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            ConditionsTermsPage ConditionsTermsPage;
            if (id == null)
            {
                ConditionsTermsPage = await GetActiveConditionsTermsPage();
                if (ConditionsTermsPage != null)
                    return View(ConditionsTermsPage);
            }
            ConditionsTermsPage = await db.ConditionsTermsPages.FindAsync(id);
            if (ConditionsTermsPage != null)
            {
                return View(ConditionsTermsPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewPrivacity = await db.PagePreviews.FindAsync("PreviewPrivacity");

            var str = new MemoryStream(previewPrivacity.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ConditionsTermsPagePreview;

            var ConditionsTermsPageActive = await GetActiveConditionsTermsPage();
            var lastIdConditionsTermsPage = await db.ConditionsTermsPages.MaxAsync(x => x.ConditionsTermsPageId) + 1;

            if (preview == null)
            {
                if (ConditionsTermsPageActive != null)
                {
                    return View("Preview", ConditionsTermsPageActive);
                }
            }
            else
            {
                var ConditionsTermsPagePreview = GetConditionsTermsPagePreview(ConditionsTermsPageActive, preview, lastIdConditionsTermsPage);
                return View("Preview", ConditionsTermsPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ConditionsTermsPage", "~/UploadedFiles", "PreviewPrivacity", new ConditionsTermsPagePreview { ConditionsTermsPageName = "PreviewPrivacity" }, null, FillDataTextConditionsTermsPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var ConditionsTermsPageActive = await GetActiveConditionsTermsPage();
            return View(ConditionsTermsPageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ConditionsTermsPage", "~/Content/rsc/imgs", "PreviewPrivacity", new ConditionsTermsPagePreview { ConditionsTermsPageName = "PreviewPrivacity" }, null, FillDataTextConditionsTermsPage, null);

            if (result.Key)
            {
                PagePreview previewPrivacity = await db.PagePreviews.FindAsync("PreviewPrivacity");

                var str = new MemoryStream(previewPrivacity.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as ConditionsTermsPagePreview;

                if (preview != null)
                {
                    var ConditionsTermsPageActive = await GetActiveConditionsTermsPage();
                    var lastIdConditionsTermsPage = await db.ConditionsTermsPages.MaxAsync(x => x.ConditionsTermsPageId) + 1;
                    var ConditionsTermsPageEdit = GetConditionsTermsPagePreview(ConditionsTermsPageActive, preview, lastIdConditionsTermsPage);
                    db.ConditionsTermsPages.Add(ConditionsTermsPageEdit);
                    await db.SaveChangesAsync();
                    ConditionsTermsPageActive.ConditionsTermsPageActive = false;
                    db.Entry(ConditionsTermsPageActive).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmDeletion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var page = db.ConditionsTermsPages.Where(p => p.ConditionsTermsPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.ConditionsTermsPageCreatedDate,
                Id = page.ConditionsTermsPageId,
                Active = page.ConditionsTermsPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.ConditionsTermsPageCustomValue,
                Title = "Eliminar página de términos y condiciones"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.ConditionsTermsPages.Where(p => p.ConditionsTermsPageId == id).FirstOrDefault();
            if (page != null && page.ConditionsTermsPageActive == false)
            {
                db.ConditionsTermsPages.Remove(page);
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

        private ConditionsTermsPage GetConditionsTermsPagePreview(ConditionsTermsPage ConditionsTermsPageActive, ConditionsTermsPagePreview preview, int lastIdConditionsTermsPage)
        {
            var ConditionsTermsPagePreview = new ConditionsTermsPage
            {
                ConditionsTermsPageCustomValue = $"ConditionsTermsPage_{lastIdConditionsTermsPage}",
                ConditionsTermsPageTitle = "Privacidad",
                ConditionsTermsPageActive = true,
                ConditionsTermsPageCreatedDate = DateTime.Now,
                ConditionsTermsPageHeadText = preview.HeadTextConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageHeadText,
                ConditionsTermsPageColorHeadText = preview.ColorHeadTextConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageColorHeadText,
                ConditionsTermsPageBgColorHead = preview.BgColorHeadConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageBgColorHead,
                ConditionsTermsPageTextColor = preview.TextColorConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageTextColor,
                ConditionsTermsPageTextDescription = preview.TextDescriptionConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageTextDescription,
                ConditionsTermsPageTextTitle = preview.TextTitleConditionsTermsPage ?? ConditionsTermsPageActive.ConditionsTermsPageTextTitle,
            };

            return ConditionsTermsPagePreview;
        }

        private async Task<ConditionsTermsPage> GetActiveConditionsTermsPage()
        {
            var h = await db.ConditionsTermsPages.FirstOrDefaultAsync(p => p.ConditionsTermsPageActive);
            return h;
        }

        private static void FillDataTextConditionsTermsPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as ConditionsTermsPagePreview;

            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "HeadTextConditionsTermsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.HeadTextConditionsTermsPage = formValue;
                        break;
                    case "ColorHeadTextConditionsTermsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorHeadTextConditionsTermsPage = formValue;
                        break;
                    case "BgColorHeadConditionsTermsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.BgColorHeadConditionsTermsPage = formValue;
                        break;
                    case "TextDescriptionConditionsTermsPage":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextDescriptionConditionsTermsPage = formValue;
                        break;
                    case "TextColorConditionsTermsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextColorConditionsTermsPage = formValue;
                        break;
                    case "TextTitleConditionsTermsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextTitleConditionsTermsPage = formValue;
                        break;
                }
            }
        }
    }
}