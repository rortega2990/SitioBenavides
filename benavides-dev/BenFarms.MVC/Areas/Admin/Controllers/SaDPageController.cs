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
    public class SaDPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var SaDPages = db.SaDPages;
            return View(await SaDPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var SaDPage = await db.SaDPages.FindAsync(id);

            if (SaDPage != null)
            {
                var SaDPageActive = await GetActiveSaDPage();
                if (SaDPageActive != null)
                {
                    if (!SaDPage.SaDPageActive)
                    {
                        SaDPage.SaDPageActive = true;
                        SaDPageActive.SaDPageActive = false;
                        db.Entry(SaDPage).State = EntityState.Modified;
                        db.Entry(SaDPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.SaDPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            SaDPage SaDPage;
            if (id == null)
            {
                SaDPage = await GetActiveSaDPage();
                if (SaDPage != null)
                    return View(SaDPage);
            }
            SaDPage = await db.SaDPages.FindAsync(id);
            if (SaDPage != null)
            {
                SaDPage.SaDTypeNumbers = GetActiveSaDTypes();
                return View(SaDPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewSaD = await db.PagePreviews.FindAsync("PreviewSaD");

            var str = new MemoryStream(previewSaD.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as SaDPagePreview;

            var SaDPageActive = await GetActiveSaDPage();
            var lastIdSaDPage = await db.SaDPages.MaxAsync(x => x.SaDPageId) + 1;

            if (preview == null)
            {
                if (SaDPageActive != null)
                    return View("Preview", SaDPageActive);
            }
            else
            {
                var SaDPagePreview = GetSaDPagePreview(SaDPageActive, preview, lastIdSaDPage);
                return View("Preview", SaDPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("SaDPage", "~/UploadedFiles", "PreviewSaD", new SaDPagePreview { SaDPageName = "PreviewSaD" }, FileType.Image, FillDataTextSaDPage, FillDataImageSaDPage);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var SaDPageActive = await GetActiveSaDPage();
            return View(SaDPageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("SaDPage", "~/Content/rsc/imgs", "PreviewSaD", new SaDPagePreview { SaDPageName = "PreviewSaD" }, FileType.Image, FillDataTextSaDPage, FillDataImageSaDPage);

            if (result.Key)
            {
                PagePreview previewSaD = await db.PagePreviews.FindAsync("PreviewSaD");

                var str = new MemoryStream(previewSaD.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as SaDPagePreview;

                if (preview != null)
                {
                    var SaDPageActive = await GetActiveSaDPage();
                    var lastIdSaDPage = await db.SaDPages.MaxAsync(x => x.SaDPageId) + 1;
                    var SaDPageEdit = GetSaDPagePreview(SaDPageActive, preview, lastIdSaDPage);
                    db.SaDPages.Add(SaDPageEdit);
                    await db.SaveChangesAsync();
                    SaDPageActive.SaDPageActive = false;
                    db.Entry(SaDPageActive).State = EntityState.Modified;
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

            var page = db.SaDPages.Where(p => p.SaDPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.SaDPageCreatedDate,
                Id = page.SaDPageId,
                Active = page.SaDPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.SaDPageCustomValue,
                Title = "Eliminar página de servicio a domicilio"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.SaDPages.Where(p => p.SaDPageId == id).FirstOrDefault();
            if (page != null && page.SaDPageActive == false)
            {
                db.SaDPages.Remove(page);
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

        private SaDPage GetSaDPagePreview(SaDPage SaDPageActive, SaDPagePreview preview, int lastIdSaDPage)
        {
            var SaDPagePreview = new SaDPage
            {
                SaDPageCustomValue = $"SaDPage_{lastIdSaDPage}",
                SaDPageTitle = "Servicio a Domicilio",
                SaDPageActive = true,
                SaDPageCreatedDate = DateTime.Now,
                SaDPageHeadText1 = preview.Texto1SaD ?? SaDPageActive.SaDPageHeadText1,
                SaDPageHeadText2 = preview.Texto2SaD ?? SaDPageActive.SaDPageHeadText2,
                SaDPageHeadTextColor1 = preview.ColorTexto1SaD ?? SaDPageActive.SaDPageHeadTextColor1,
                SaDPageHeadTextColor2 = preview.ColorTexto2SaD ?? SaDPageActive.SaDPageHeadTextColor2,
                SaDPageImageBg = preview.ImageBgSaD ?? SaDPageActive.SaDPageImageBg,
                SaDPageImageLogo = preview.ImagenLogoSaD ?? SaDPageActive.SaDPageImageLogo,
                SaDPageSubText1 = preview.TextoTituloSaD ?? SaDPageActive.SaDPageSubText1,
                SaDPageSubTextColor1 = preview.ColorTextoTituloSaD ?? SaDPageActive.SaDPageSubTextColor1,
                SaDPageNumberPrincipalText = preview.NumeroprincipalSaD ?? SaDPageActive.SaDPageNumberPrincipalText,
                SaDPageNumberPrincipalBgColor =
                    preview.ColorBgNumeroprincipalSaD ?? SaDPageActive.SaDPageNumberPrincipalBgColor,
                SaDPageNumberPrincipalTextColor =
                    preview.ColorNumeroprincipalSaD ?? SaDPageActive.SaDPageNumberPrincipalTextColor,
                SaDTypeNumbers = GetActiveSaDTypes(),
            };

            return SaDPagePreview;
        }

        private async Task<SaDPage> GetActiveSaDPage()
        {
            var h = await db.SaDPages.FirstOrDefaultAsync(p => p.SaDPageActive);
            if (h != null)
            {
                h.SaDTypeNumbers = GetActiveSaDTypes();
                return h;
            }
            return null;
        }

        private List<SaDTypeNumber> GetActiveSaDTypes()
        {
            return db.SaDTypeNumbers.Where(x => x.SaDTypeNumberActive).ToList();
        }

        private static void FillDataImageSaDPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as SaDPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImageBgSaD":
                        preview.ImageBgSaD = fileName;
                        break;
                    case "ImagenLogoSaD":
                        preview.ImagenLogoSaD = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextSaDPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as SaDPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "Texto1SaD":
                        preview.Texto1SaD = formValue;
                        break;
                    case "Texto2SaD":
                        preview.Texto2SaD = formValue;
                        break;
                    case "ColorTexto1SaD":
                        preview.ColorTexto1SaD = formValue;
                        break;
                    case "ColorTexto2SaD":
                        preview.ColorTexto2SaD = formValue;
                        break;
                    case "TextoTituloSaD":
                        preview.TextoTituloSaD = formValue;
                        break;
                    case "ColorTextoTituloSaD":
                        preview.ColorTextoTituloSaD = formValue;
                        break;
                    case "NumeroprincipalSaD":
                        preview.NumeroprincipalSaD = formValue;
                        break;
                    case "ColorNumeroprincipalSaD":
                        preview.ColorNumeroprincipalSaD = formValue;
                        break;
                    case "ColorBgNumeroprincipalSaD":
                        preview.ColorBgNumeroprincipalSaD = formValue;
                        break;
                }
            }
        }

    }
}