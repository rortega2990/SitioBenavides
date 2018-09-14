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
    public class BranchPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var BranchPages = db.BranchPages;
            return View(await BranchPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var BranchPage = await db.BranchPages.FindAsync(id);

            if (BranchPage != null)
            {
                var BranchPageActive = await GetActiveBranchPage();
                if (BranchPageActive != null)
                {
                    if (!BranchPage.BranchPageActive)
                    {
                        BranchPage.BranchPageActive = true;
                        BranchPageActive.BranchPageActive = false;
                        db.Entry(BranchPage).State = EntityState.Modified;
                        db.Entry(BranchPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.BranchPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            BranchPage BranchPage;
            if (id == null)
            {
                BranchPage = await GetActiveBranchPage();
                if (BranchPage != null)
                    return View(BranchPage);
            }
            BranchPage = await db.BranchPages.FindAsync(id);
            if (BranchPage != null)
            {
                BranchPage.Branchs = await GetActiveBranchs();
                BranchPage.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == BranchPage.BranchPageId && x.ImageSectionPageName == "BranchPage").ToListAsync();
                return View(BranchPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewBranch = await db.PagePreviews.FindAsync("PreviewBranch");

            var str = new MemoryStream(previewBranch.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as BranchPagePreview;

            var BranchPageActive = await GetActiveBranchPage();
            var lastIdBranchPage = await db.BranchPages.MaxAsync(x => x.BranchPageId) + 1;

            if (preview == null)
            {
                if (BranchPageActive != null)
                    return View("Preview", BranchPageActive);
            }
            else
            {
                var BranchPagePreview = await GetBranchPagePreview(BranchPageActive, preview, lastIdBranchPage);
                return View("Preview", BranchPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BranchPage", "~/UploadedFiles", "PreviewBranch", new BranchPagePreview { BranchPageName = "PreviewBranch" }, FileType.Image, FillDataTextBranchPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var branch = await GetActiveBranchPage();
            return View(branch);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BranchPage", "~/UploadedFiles", "PreviewBranch", new BranchPagePreview { BranchPageName = "PreviewBranch" }, FileType.Image, FillDataTextBranchPage, null);

            if (result.Key)
            {
                PagePreview previewBranch = await db.PagePreviews.FindAsync("PreviewBranch");

                var str = new MemoryStream(previewBranch.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as BranchPagePreview;

                if (preview != null)
                {
                    var BranchPageActive = await GetActiveBranchPage();
                    var lastIdBranchPage = await db.BranchPages.MaxAsync(x => x.BranchPageId) + 1;
                    var BranchPageEdit = await GetBranchPagePreview(BranchPageActive, preview, lastIdBranchPage);
                    var id = db.BranchPages.Add(BranchPageEdit);
                    await db.SaveChangesAsync();
                    foreach (var h in BranchPageEdit.HeadImages)
                    {
                        h.ImageSectionPageId = id.BranchPageId;
                        db.ImageSections.Add(h);
                    }
                    await db.SaveChangesAsync();
                    BranchPageActive.BranchPageActive = false;
                    db.Entry(BranchPageActive).State = EntityState.Modified;
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

            var page = db.BranchPages.Where(p => p.BranchPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.BranchPageCreatedDate,
                Id = page.BranchPageId,
                Active = page.BranchPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.BranchPageCustomValue,
                Title = "Eliminar página de sucursales"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.BranchPages.Where(p => p.BranchPageId == id).FirstOrDefault();
            if (page != null && page.BranchPageActive == false)
            {
                db.BranchPages.Remove(page);
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

        private async Task<BranchPage> GetBranchPagePreview(BranchPage BranchPageActive, BranchPagePreview preview, int lastIdBranchPage)
        {
            var BranchPagePreview = new BranchPage
            {
                BranchPageCustomValue = $"BranchPage_{lastIdBranchPage}",
                BranchPageTitle = "Sucursales",
                BranchPageActive = true,
                BranchPageCreatedDate = DateTime.Now,
                BranchPageMessage = preview.TextoSucursales1 ?? BranchPageActive.BranchPageMessage,
                BranchPageBranchNames = preview.TextoSucursales2 ?? BranchPageActive.BranchPageBranchNames,
                Branchs = await GetActiveBranchs(),
                BranchPageColorMessage = preview.ColorTextoSucursales1 ?? BranchPageActive.BranchPageColorMessage,
                BranchPageColorTextBranchNames = preview.ColorTextoSucursales2 ?? BranchPageActive.BranchPageColorTextBranchNames,
                HeadImages = Utils.ConvertToImageSectionList(preview.Encabezado)
            };

            return BranchPagePreview;
        }

        private async Task<BranchPage> GetActiveBranchPage()
        {
            var h = await db.BranchPages.FirstOrDefaultAsync(p => p.BranchPageActive);
            if (h != null)
            {
                h.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == h.BranchPageId && x.ImageSectionPageName == "BranchPage").ToListAsync();
                h.Branchs = await GetActiveBranchs();
                return h;
            }
            return null;
        }

        private async Task<List<Branch>> GetActiveBranchs()
        {
            return await db.Branchs.Where(x => x.BranchActive && x.BranchConsult).ToListAsync();
        }

        private static void FillDataTextBranchPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as BranchPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "TextoSucursales1":
                        preview.TextoSucursales1 = formValue;
                        break;
                    case "TextoSucursales2":
                        preview.TextoSucursales2 = formValue;
                        break;
                    case "ColorTextoSucursales1":
                        preview.ColorTextoSucursales1 = formValue;
                        break;
                    case "ColorTextoSucursales2":
                        preview.ColorTextoSucursales2 = formValue;
                        break;
                }
            }
        }
    }
}