using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using BenFarms.MVC.Models;
using System.Web;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BillingPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var BillingPages = db.BillingPages.Include(h => h.BenefitSection).Include(h => h.IncrementBenefitSection).Include(h => h.LabSection);
            return View(await BillingPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var BillingPage = await db.BillingPages.FindAsync(id);

            if (BillingPage != null)
            {
                var BillingPageActive = await GetActiveBillingPage();
                if (BillingPageActive != null)
                {
                    if (!BillingPage.BillingPageActive)
                    {
                        BillingPage.BillingPageActive = true;
                        BillingPageActive.BillingPageActive = false;
                        db.Entry(BillingPage).State = EntityState.Modified;
                        db.Entry(BillingPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        var BillingPages = db.BillingPages.Include(h => h.BenefitSection).Include(h => h.IncrementBenefitSection).Include(h => h.LabSection);
                        return View("Index", await BillingPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            BillingPage BillingPage;
            if (id == null)
            {
                BillingPage = await GetActiveBillingPage();
                if (BillingPage != null)
                    return View(BillingPage);
            }
            BillingPage = await db.BillingPages.FindAsync(id);
            if (BillingPage != null)
            {
                BillingPage.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == BillingPage.BillingPageId && x.ImageSectionPageName == "BillingPage").ToListAsync();
                BillingPage.LabSection.ImageSections = await db.ImageSections.Where(x => x.ImageSectionPageId == BillingPage.LabSectionId && x.ImageSectionPageName == "LabSection").ToListAsync();
                return View(BillingPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewBilling = await db.PagePreviews.FindAsync("PreviewBilling");

            var str = new MemoryStream(previewBilling.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as BillingPagePreview;

            var BillingPageActive = await GetActiveBillingPage();
            var lastIdBillingPage = await db.BillingPages.MaxAsync(x => x.BillingPageId) + 1;
            var lastIdLab = await db.LabSections.MaxAsync(x => x.LabSectionId);
            var lastIdBenefict = await db.BenefitSections.MaxAsync(x => x.BenefitSectionId);
            var lastIdIncrementBenefit = await db.IncrementBenefitSections.MaxAsync(x => x.IncrementBenefitSectionId);

            if (preview == null)
            {
                if (BillingPageActive != null)
                    return View("Preview", BillingPageActive);
            }
            else
            {
                var BillingPagePreview = GetBillingPagePreview(BillingPageActive, preview, lastIdBillingPage, lastIdBenefict, lastIdIncrementBenefit, lastIdLab);
                return View("Preview", BillingPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BillingPage", "~/UploadedFiles", "PreviewBilling", new BillingPagePreview { BillingPageName = "PreviewBilling" }, FileType.Image, FillDataTextCardPage, FillDataImageCardPage);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var billing = await GetActiveBillingPage();
            return View(billing);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BillingPage", "~/Content/rsc/imgs", "PreviewBilling", new BillingPagePreview { BillingPageName = "PreviewBilling" }, FileType.Image, FillDataTextCardPage, FillDataImageCardPage);

            if (result.Key)
            {
                PagePreview previewBilling = await db.PagePreviews.FindAsync("PreviewBilling");

                var str = new MemoryStream(previewBilling.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as BillingPagePreview;

                if (preview != null)
                {
                    var BillingPageActive = await GetActiveBillingPage();
                    var lastIdBillingPage = await db.BillingPages.MaxAsync(x => x.BillingPageId) + 1;
                    var lastIdLab = await db.LabSections.MaxAsync(x => x.LabSectionId);
                    var lastIdBenefict = await db.BenefitSections.MaxAsync(x => x.BenefitSectionId);
                    var lastIdIncrementBenefit = await db.IncrementBenefitSections.MaxAsync(x => x.IncrementBenefitSectionId);
                    var BillingPageEdit = GetBillingPagePreview(BillingPageActive, preview, lastIdBillingPage, lastIdBenefict, lastIdIncrementBenefit, lastIdLab);
                    var id = db.BillingPages.Add(BillingPageEdit);
                    await db.SaveChangesAsync();
                    foreach (var h in BillingPageEdit.HeadImages)
                    {
                        h.ImageSectionPageId = id.BillingPageId;
                        db.ImageSections.Add(h);
                    }
                    await db.SaveChangesAsync();
                    foreach (var h in BillingPageEdit.LabSection.ImageSections)
                    {
                        h.ImageSectionPageId = id.LabSectionId;
                        db.ImageSections.Add(h);
                    }
                    await db.SaveChangesAsync();
                    BillingPageActive.BillingPageActive = false;
                    db.Entry(BillingPageActive).State = EntityState.Modified;
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

            var page = db.BillingPages.Where(p => p.BillingPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.BillingPageCreatedDate,
                Id = page.BillingPageId,
                Active = page.BillingPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.BillingPageCustomValue,
                Title = "Eliminar página de tarjetas"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.BillingPages.Where(p => p.BillingPageId == id).FirstOrDefault();
            if (page != null && page.BillingPageActive == false)
            {
                db.BillingPages.Remove(page);
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

        private BillingPage GetBillingPagePreview(BillingPage BillingPageActive, BillingPagePreview preview, int lastIdBillingPage, int lastIdBenefict, int lastIdIncrementBenefit, int lastIdLab)
        {
            var BillingPagePreview = new BillingPage
            {
                BillingPageCustomValue = $"BillingPage_{lastIdBillingPage}",
                BillingPageTitle = "Facturación en línea",
                BillingPageActive = true,
                BillingPageCreatedDate = DateTime.Now,
                BenefitSection = new BenefitSection
                {
                    BenefitSectionCustomValue = $"BenefitSection_{++lastIdBenefict}",
                    BenefitSectionDiv = preview.BeneficiosTarjeta ?? BillingPageActive.BenefitSection.BenefitSectionDiv,
                    BenefitSectionParagraph =
                        preview.BeneficiosTarjetaParrafo ?? BillingPageActive.BenefitSection.BenefitSectionParagraph,
                    BenefitSectionImage = preview.BeneficiosTarjetaImagen ?? BillingPageActive.BenefitSection.BenefitSectionImage,
                    BenefitSectionImageXS = preview.BeneficiosTarjetaImagenXs ?? BillingPageActive.BenefitSection.BenefitSectionImageXS,
                },
                IncrementBenefitSection = new IncrementBenefitSection
                {
                    IncrementBenefitSectionCustomValue = $"IncrementBenefitSection_{++lastIdIncrementBenefit}",
                    IncrementBenefitSectionDiv =
                        preview.AumentaBeneficiosTarjeta ??
                        BillingPageActive.IncrementBenefitSection.IncrementBenefitSectionDiv,
                    IncrementBenefitSectionImage1 = preview.AumentaBeneficiosImagen1 ??
                        BillingPageActive.IncrementBenefitSection.IncrementBenefitSectionImage1,
                    IncrementBenefitSectionImage2 = preview.AumentaBeneficiosImagen2 ??
                        BillingPageActive.IncrementBenefitSection.IncrementBenefitSectionImage2,
                },
                LabSection = new LabSection
                {
                    LabSectionCustomValue = $"LabSection_{++lastIdLab}",
                    LabSectionTitle = preview.TituloLaboratorios ?? BillingPageActive.LabSection.LabSectionTitle,
                    ImageSections = Utils.ConvertToImageSectionList(preview.ImagesLab),
                },
                HeadImages = Utils.ConvertToImageSectionList(preview.Encabezado),
            };

            return BillingPagePreview;
        }

        private async Task<BillingPage> GetActiveBillingPage()
        {
            var h = await db.BillingPages.FirstOrDefaultAsync(p => p.BillingPageActive);
            if (h != null)
            {
                h.HeadImages =
                    await
                        db.ImageSections.Where(
                            x => x.ImageSectionPageId == h.BillingPageId && x.ImageSectionPageName == "BillingPage")
                            .ToListAsync();
                h.LabSection.ImageSections =
                    await
                        db.ImageSections.Where(
                            x => x.ImageSectionPageId == h.LabSectionId && x.ImageSectionPageName == "LabSection")
                            .ToListAsync();
                return h;
            }
            return null;
        }

        private static void FillDataImageCardPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as BillingPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "BeneficiosTarjetaImagen":
                        preview.BeneficiosTarjetaImagen = fileName;
                        break;
                    case "BeneficiosTarjetaImagenXs":
                        preview.BeneficiosTarjetaImagenXs = fileName;
                        break;
                    case "AumentaBeneficiosImagen1":
                        preview.AumentaBeneficiosImagen1 = fileName;
                        break;
                    case "AumentaBeneficiosImagen2":
                        preview.AumentaBeneficiosImagen2 = fileName;
                        break;                    
                }
            }
        }

        private static void FillDataTextCardPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as BillingPagePreview;
            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "BeneficiosTarjeta":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.BeneficiosTarjeta = formValue;
                        break;
                    case "BeneficiosTarjetaParrafo":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.BeneficiosTarjetaParrafo = formValue;
                        break;
                    case "AumentaBeneficiosTarjeta":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.AumentaBeneficiosTarjeta = formValue;
                        break;
                    case "TituloLaboratorios":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TituloLaboratorios = formValue;
                        break;
                }
            }
        }
    }
}