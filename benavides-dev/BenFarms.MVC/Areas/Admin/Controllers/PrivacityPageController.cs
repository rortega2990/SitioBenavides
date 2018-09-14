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
    public class PrivacityPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var PrivacityPages = db.PrivacityPages;
            return View(await PrivacityPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var PrivacityPage = await db.PrivacityPages.FindAsync(id);

            if (PrivacityPage != null)
            {
                var PrivacityPageActive = await GetActivePrivacityPage();
                if (PrivacityPageActive != null)
                {
                    if (!PrivacityPage.PrivacityPageActive)
                    {
                        PrivacityPage.PrivacityPageActive = true;
                        PrivacityPageActive.PrivacityPageActive = false;
                        db.Entry(PrivacityPage).State = EntityState.Modified;
                        db.Entry(PrivacityPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.PrivacityPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            PrivacityPage PrivacityPage;
            if (id == null)
            {
                PrivacityPage = await GetActivePrivacityPage();
                if (PrivacityPage != null)
                    return View(PrivacityPage);
            }
            PrivacityPage = await db.PrivacityPages.FindAsync(id);
            if (PrivacityPage != null)
            {
                return View(PrivacityPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewPrivacity = await db.PagePreviews.FindAsync("PreviewPrivacity");

            var str = new MemoryStream(previewPrivacity.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as PrivacityPagePreview;

            var PrivacityPageActive = await GetActivePrivacityPage();
            var lastIdPrivacityPage = await db.PrivacityPages.MaxAsync(x => x.PrivacityPageId) + 1;

            if (preview == null)
            {
                if (PrivacityPageActive != null)
                {
                    return View("Preview", PrivacityPageActive);
                }
            }
            else
            {
                var PrivacityPagePreview = GetPrivacityPagePreview(PrivacityPageActive, preview, lastIdPrivacityPage);
                return View("Preview", PrivacityPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("PrivacityPage", "~/UploadedFiles", "PreviewPrivacity", new PrivacityPagePreview { PrivacityPageName = "PreviewPrivacity" }, null, FillDataTextPrivacityPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var PrivacityPageActive = await GetActivePrivacityPage();
            return View(PrivacityPageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("PrivacityPage", "~/Content/rsc/imgs", "PreviewPrivacity", new PrivacityPagePreview { PrivacityPageName = "PreviewPrivacity" }, null, FillDataTextPrivacityPage, null);

            if (result.Key)
            {
                PagePreview previewPrivacity = await db.PagePreviews.FindAsync("PreviewPrivacity");

                var str = new MemoryStream(previewPrivacity.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as PrivacityPagePreview;

                if (preview != null)
                {
                    var PrivacityPageActive = await GetActivePrivacityPage();
                    var lastIdPrivacityPage = await db.PrivacityPages.MaxAsync(x => x.PrivacityPageId) + 1;
                    var PrivacityPageEdit = GetPrivacityPagePreview(PrivacityPageActive, preview, lastIdPrivacityPage);
                    db.PrivacityPages.Add(PrivacityPageEdit);
                    await db.SaveChangesAsync();
                    PrivacityPageActive.PrivacityPageActive = false;
                    db.Entry(PrivacityPageActive).State = EntityState.Modified;
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

            var page = db.PrivacityPages.Where(p => p.PrivacityPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.PrivacityPageCreatedDate,
                Id = page.PrivacityPageId,
                Active = page.PrivacityPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.PrivacityPageCustomValue,
                Title = "Eliminar página de privacidad"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.PrivacityPages.Where(p => p.PrivacityPageId == id).FirstOrDefault();
            if (page != null && page.PrivacityPageActive == false)
            {
                db.PrivacityPages.Remove(page);
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

        private PrivacityPage GetPrivacityPagePreview(PrivacityPage PrivacityPageActive, PrivacityPagePreview preview, int lastIdPrivacityPage)
        {
            var PrivacityPagePreview = new PrivacityPage
            {
                PrivacityPageCustomValue = $"PrivacityPage_{lastIdPrivacityPage}",
                PrivacityPageTitle = "Privacidad",
                PrivacityPageActive = true,
                PrivacityPageCreatedDate = DateTime.Now,
                PrivacityPageHeadText = preview.HeadTextPrivacityPage ?? PrivacityPageActive.PrivacityPageHeadText,
                PrivacityPageColorHeadText = preview.ColorHeadTextPrivacityPage ?? PrivacityPageActive.PrivacityPageColorHeadText,
                PrivacityPageBgColorHead = preview.BgColorHeadPrivacityPage ?? PrivacityPageActive.PrivacityPageBgColorHead,
                PrivacityPageTextColor = preview.TextColorPrivacityPage ?? PrivacityPageActive.PrivacityPageTextColor,
                PrivacityPageTextDescription = preview.TextDescriptionPrivacityPage ?? PrivacityPageActive.PrivacityPageTextDescription,
                PrivacityPageTextTitle = preview.TextTitlePrivacityPage ?? PrivacityPageActive.PrivacityPageTextTitle,
            };

            return PrivacityPagePreview;
        }

        private async Task<PrivacityPage> GetActivePrivacityPage()
        {
            var h = await db.PrivacityPages.FirstOrDefaultAsync(p => p.PrivacityPageActive);
            return h;
        }

        private static void FillDataTextPrivacityPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as PrivacityPagePreview;
            
            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "HeadTextPrivacityPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.HeadTextPrivacityPage = formValue;
                        break;
                    case "ColorHeadTextPrivacityPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorHeadTextPrivacityPage = formValue;
                        break;
                    case "BgColorHeadPrivacityPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.BgColorHeadPrivacityPage = formValue;
                        break;
                    case "TextDescriptionPrivacityPage":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextDescriptionPrivacityPage = formValue;
                        break;
                    case "TextColorPrivacityPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextColorPrivacityPage = formValue;
                        break;
                    case "TextTitlePrivacityPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextTitlePrivacityPage = formValue;
                        break;
                }
            }
        }
    }
}