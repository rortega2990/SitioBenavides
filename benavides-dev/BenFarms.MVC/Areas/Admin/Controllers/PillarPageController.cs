using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using System.Web;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PillarPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var pillarPages = db.PillarPages;
            return View(await pillarPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var pillarPage = await db.PillarPages.FindAsync(id);

            if (pillarPage != null)
            {
                var pillarPageActive = await GetActivePillarPage();
                if (pillarPageActive != null)
                {
                    if (!pillarPage.PillarPageActive)
                    {
                        pillarPage.PillarPageActive = true;
                        pillarPageActive.PillarPageActive = false;
                        db.Entry(pillarPage).State = EntityState.Modified;
                        db.Entry(pillarPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.PillarPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            PillarPage pillarPage;
            if (id == null)
            {
                pillarPage = await GetActivePillarPage();
                if (pillarPage != null)
                    return View(pillarPage);
            }
            pillarPage = await db.PillarPages.FindAsync(id);
            if (pillarPage != null)
            {
                pillarPage.Pillars = await GetActivePillars();
                pillarPage.Quotes = await GetQuotes();
                return View(pillarPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewPillar = await db.PagePreviews.FindAsync("PreviewPillar");

            var str = new MemoryStream(previewPillar.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as PillarPagePreview;

            var pillarPageActive = await GetActivePillarPage();
            var lastIdPillarPage = await db.PillarPages.MaxAsync(x => x.PillarPageId) + 1;

            if (preview == null)
            {
                if (pillarPageActive != null)
                    return View("Preview", pillarPageActive);
            }
            else
            {
                var pillarPagePreview = await GetPillarPagePreview(preview);
                return View("Preview", pillarPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("PillarPage", "~/UploadedFiles", "PreviewPillar", new PillarPagePreview { PillarPageName = "PreviewPillar" }, FileType.Image, FillDataTextPillarPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var pillar = await GetActivePillarPage();
            return View(pillar);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("PillarPage", "~/Content/rsc/imgs", "PreviewPillar", new PillarPagePreview { PillarPageName = "PreviewPillar" }, FileType.ImagePdf, FillDataTextPillarPage, FillDataImagePillarPage);

            if (result.Key)
            {
                PagePreview previewPillar = await db.PagePreviews.FindAsync("PreviewPillar");

                var str = new MemoryStream(previewPillar.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as PillarPagePreview;

                if (preview != null)
                {
                    var pillarPageActive = await GetActivePillarPage();
                    var lastIdPillarPage = await db.PillarPages.MaxAsync(x => x.PillarPageId) + 1;
                    var pillarPageEdit = await GetPillarPagePreview(preview);
                    var id = db.PillarPages.Add(pillarPageEdit);
                    await db.SaveChangesAsync();
                    pillarPageActive.PillarPageActive = false;
                    db.Entry(pillarPageActive).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        private static void FillDataImagePillarPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as PillarPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImagePillarPage":
                        preview.ImagenPaginaPilares = fileName;
                        break;
                }
            }
        }

        public ActionResult ConfirmDeletion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var page = db.PillarPages.Where(p => p.PillarPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.PillarPageCreatedDate,
                Id = page.PillarPageId,
                Active = page.PillarPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.PillarPageCustomValue,
                Title = "Eliminar página de pilares"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.PillarPages.Where(p => p.PillarPageId == id).FirstOrDefault();
            if (page != null && page.PillarPageActive == false)
            {
                db.PillarPages.Remove(page);
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

        private async Task<PillarPage> GetPillarPagePreview(PillarPagePreview preview)
        {
            var pillarPageActive = await GetActivePillarPage();
            var lastIdPillarPage = await db.PillarPages.MaxAsync(x => x.PillarPageId) + 1;
            var pillarPagePreview = new PillarPage
            {
                PillarPageCustomValue = $"PillarPage_{lastIdPillarPage}",
                PillarPageTitle = "Pilares",
                PillarPageActive = true,
                PillarPageCreatedDate = DateTime.Now,
                PillarPageText1 = preview.TextoPilares1 ?? pillarPageActive.PillarPageText1,
                PillarPageText2 = preview.TextoPilares2 ?? pillarPageActive.PillarPageText2,
                Pillars = await GetActivePillars(),
                Quotes = await GetQuotes(),
                PillarPageImage = preview.ImagenPaginaPilares ?? pillarPageActive.PillarPageImage
            };

            return pillarPagePreview;
        }

        private async Task<PillarPage> GetActivePillarPage()
        {
            var h = await db.PillarPages.FirstOrDefaultAsync(p => p.PillarPageActive);
            if (h != null)
            {
                h.Pillars = await GetActivePillars();
                h.Quotes = await GetQuotes();
                return h;
            }
            return null;
        }

        private async Task<List<Pillar>> GetActivePillars()
        {
            return await db.Pillars.Where(x => x.PillarActive).ToListAsync();
        }

        private async Task<List<Quote>> GetQuotes()
        {
            return await db.Quotes.ToListAsync();
        }

        private static void FillDataTextPillarPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as PillarPagePreview;
            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "TextPillarPageText1":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextoPilares1 = formValue;
                        break;
                    case "TextPillarPageText2":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextoPilares2 = System.Net.WebUtility.HtmlDecode(formValue);
                        break;
                    case "ImagePillarPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        if (formValue != null)
                            preview.ImagenPaginaPilares = "~/" + formValue.Substring(formValue.IndexOf("Content", StringComparison.Ordinal));
                        break;
                }
            }
        }
    }
}