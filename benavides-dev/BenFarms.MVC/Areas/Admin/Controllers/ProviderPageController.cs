using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using System.Web;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProviderPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ProviderPages = db.ProviderPages;
            return View(await ProviderPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var ProviderPage = await db.ProviderPages.FindAsync(id);

            if (ProviderPage != null)
            {
                var ProviderPageActive = await GetActiveProviderPage();
                if (ProviderPageActive != null)
                {
                    if (!ProviderPage.ProviderPageActive)
                    {
                        ProviderPage.ProviderPageActive = true;
                        ProviderPageActive.ProviderPageActive = false;
                        db.Entry(ProviderPage).State = EntityState.Modified;
                        db.Entry(ProviderPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.ProviderPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            ProviderPage ProviderPage;
            if (id == null)
            {
                ProviderPage = await GetActiveProviderPage();
                if (ProviderPage != null)
                    return View(ProviderPage);
            }
            ProviderPage = await db.ProviderPages.FindAsync(id);
            if (ProviderPage != null)
            {
                ProviderPage.DocumentTypes = GetActiveDocuments();
                return View(ProviderPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewProvider = await db.PagePreviews.FindAsync("PreviewProvider");

            var str = new MemoryStream(previewProvider.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ProviderPagePreview;

            var ProviderPageActive = await GetActiveProviderPage();
            var lastIdProviderPage = await db.ProviderPages.MaxAsync(x => x.ProviderPageId) + 1;

            if (preview == null)
            {
                if (ProviderPageActive != null)
                {
                    ProviderPageActive.DocumentTypes = GetActiveDocuments();
                    return View("Preview", ProviderPageActive);
                }
            }
            else
            {
                var ProviderPagePreview = GetProviderPagePreview(ProviderPageActive, preview, lastIdProviderPage);
                return View("Preview", ProviderPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ProviderPage", "~/UploadedFiles", "PreviewProvider", new ProviderPagePreview { ProviderPageName = "PreviewProvider" }, null, FillDataTextProviderPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var ProviderPageActive = await GetActiveProviderPage();
            return View(ProviderPageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ProviderPage", "~/Content/rsc/imgs", "PreviewProvider", new ProviderPagePreview { ProviderPageName = "PreviewProvider" }, null, FillDataTextProviderPage, null);

            if (result.Key)
            {
                PagePreview previewProvider = await db.PagePreviews.FindAsync("PreviewProvider");

                var str = new MemoryStream(previewProvider.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as ProviderPagePreview;

                if (preview != null)
                {
                    var ProviderPageActive = await GetActiveProviderPage();
                    var lastIdProviderPage = await db.ProviderPages.MaxAsync(x => x.ProviderPageId) + 1;
                    var ProviderPageEdit = GetProviderPagePreview(ProviderPageActive, preview, lastIdProviderPage);
                    db.ProviderPages.Add(ProviderPageEdit);
                    await db.SaveChangesAsync();
                    ProviderPageActive.ProviderPageActive = false;
                    db.Entry(ProviderPageActive).State = EntityState.Modified;
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

            var page = db.ProviderPages.Where(p => p.ProviderPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.ProviderPageCreatedDate,
                Id = page.ProviderPageId,
                Active = page.ProviderPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.ProviderPageCustomValue,
                Title = "Eliminar página de proveedores"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.ProviderPages.Where(p => p.ProviderPageId == id).FirstOrDefault();
            if (page != null && page.ProviderPageActive == false)
            {
                db.ProviderPages.Remove(page);
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

        private ProviderPage GetProviderPagePreview(ProviderPage ProviderPageActive, ProviderPagePreview preview, int lastIdProviderPage)
        {
            var ProviderPagePreview = new ProviderPage
            {
                ProviderPageCustomValue = $"ProviderPage_{lastIdProviderPage}",
                ProviderPageTitle = "Proveedores",
                ProviderPageActive = true,
                ProviderPageCreatedDate = DateTime.Now,
                ProviderPageHeadText = preview.HeadTextProviderPage ?? ProviderPageActive.ProviderPageHeadText,
                ProviderPageBgColorHead = preview.BgColorHeadProviderPage ?? ProviderPageActive.ProviderPageBgColorHead,
                ProviderPageColorHeadText = preview.ColorHeadTextProviderPage ?? ProviderPageActive.ProviderPageColorHeadText,
                ProviderPageSubText = preview.SubText1ProviderPage ?? ProviderPageActive.ProviderPageSubText,
                ProviderPageColorSubText = preview.ColorSubTextProviderPage ?? ProviderPageActive.ProviderPageColorSubText,
                DocumentTypes = GetActiveDocuments(),
            };

            return ProviderPagePreview;
        }

        private async Task<ProviderPage> GetActiveProviderPage()
        {
            var h = await db.ProviderPages.FirstOrDefaultAsync(p => p.ProviderPageActive);
            if (h != null)
            {
                h.DocumentTypes = GetActiveDocuments();
                return h;
            }
            return null;
        }

        private List<DocumentType> GetActiveDocuments()
        {
            return db.DocumentTypes.Where(x => x.DocumentActive).ToList();
        }

        private static void FillDataTextProviderPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as ProviderPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "HeadTextProviderPage":
                        preview.HeadTextProviderPage = formValue;
                        break;
                    case "ColorHeadTextProviderPage":
                        preview.ColorHeadTextProviderPage = formValue;
                        break;
                    case "BgColorHeadProviderPage":
                        preview.BgColorHeadProviderPage = formValue;
                        break;
                    case "SubText1ProviderPage":
                        preview.SubText1ProviderPage = formValue;
                        break;
                    case "ColorSubTextProviderPage":
                        preview.ColorSubTextProviderPage = formValue;
                        break;
                }
            }
        }
    }
}