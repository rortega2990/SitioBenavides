using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenFarms.MVC.Models;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Web;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InvestorPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var InvestorPages = db.InvestorPages;
            return View(await InvestorPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var InvestorPage = await db.InvestorPages.FindAsync(id);

            if (InvestorPage != null)
            {
                var InvestorPageActive = await GetActiveInvestorPage();
                if (InvestorPageActive != null)
                {
                    if (!InvestorPage.InvestorPageActive)
                    {
                        InvestorPage.InvestorPageActive = true;
                        InvestorPageActive.InvestorPageActive = false;
                        db.Entry(InvestorPage).State = EntityState.Modified;
                        db.Entry(InvestorPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.InvestorPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            InvestorPage InvestorPage;
            if (id == null)
            {
                InvestorPage = await GetActiveInvestorPage();
                if (InvestorPage != null)
                    return View(InvestorPage);
            }
            InvestorPage = await db.InvestorPages.FindAsync(id);
            if (InvestorPage != null)
            {
                InvestorPage.ReportTypes = GetActiveReports();
                return View(InvestorPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewInvestor = await db.PagePreviews.FindAsync("PreviewInvestor");

            var str = new MemoryStream(previewInvestor.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as InvestorPagePreview;

            var InvestorPageActive = await GetActiveInvestorPage();
            var lastIdInvestorPage = await db.InvestorPages.MaxAsync(x => x.InvestorPageId) + 1;

            if (preview == null)
            {
                if (InvestorPageActive != null)
                {
                    InvestorPageActive.ReportTypes = GetActiveReports();
                    return View("Preview", InvestorPageActive);
                }
            }
            else
            {
                var InvestorPagePreview = GetInvestorPagePreview(InvestorPageActive, preview, lastIdInvestorPage);
                return View("Preview", InvestorPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("InvestorPage", "~/UploadedFiles", "PreviewInvestor", new InvestorPagePreview { InvestorPageName = "PreviewInvestor" }, null, FillDataTextInvestorPage, null);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var InvestorPage = await GetActiveInvestorPage();
            return View(InvestorPage);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("InvestorPage", "~/Content/rsc/imgs", "PreviewInvestor", new InvestorPagePreview { InvestorPageName = "PreviewInvestor" }, null, FillDataTextInvestorPage, null);

            if (result.Key)
            {
                PagePreview previewInvestor = await db.PagePreviews.FindAsync("PreviewInvestor");

                var str = new MemoryStream(previewInvestor.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as InvestorPagePreview;

                if (preview != null)
                {
                    var InvestorPageActive = await GetActiveInvestorPage();
                    var lastIdInvestorPage = await db.InvestorPages.MaxAsync(x => x.InvestorPageId) + 1;
                    var InvestorPageEdit = GetInvestorPagePreview(InvestorPageActive, preview, lastIdInvestorPage);
                    db.InvestorPages.Add(InvestorPageEdit);
                    await db.SaveChangesAsync();
                    InvestorPageActive.InvestorPageActive = false;
                    db.Entry(InvestorPageActive).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return
                        Json(
                            new AjaxResponse
                            {
                                Success = true,
                                Message = "Los cambios se aplicaron correctamente en el servidor"
                            },
                            JsonRequestBehavior.AllowGet);
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

            var page = db.InvestorPages.Where(p => p.InvestorPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.InvestorPageCreatedDate,
                Id = page.InvestorPageId,
                Active = page.InvestorPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.InvestorPageCustomValue,
                Title = "Eliminar página de inversionistas"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.InvestorPages.Where(p => p.InvestorPageId == id).FirstOrDefault();
            if (page != null && page.InvestorPageActive == false)
            {
                db.InvestorPages.Remove(page);
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

        private InvestorPage GetInvestorPagePreview(InvestorPage InvestorPageActive, InvestorPagePreview preview, int lastIdInvestorPage)
        {
            var InvestorPagePreview = new InvestorPage
            {
                InvestorPageCustomValue = $"InvestorPage_{lastIdInvestorPage}",
                InvestorPageTitle = "Inversionistas",
                InvestorPageActive = true,
                InvestorPageCreatedDate = DateTime.Now,
                InvestorPageHeadText = preview.HeadTextInvestorPage ?? InvestorPageActive.InvestorPageHeadText,
                InvestorPageColorHeadText =
                    preview.ColorHeadTextInvestorPage ?? InvestorPageActive.InvestorPageColorHeadText,
                InvestorPageColorHeadBg = preview.ColorHeadBgInvestorPage ?? InvestorPageActive.InvestorPageColorHeadBg,
                InvestorPageSubText = preview.SubTextInvestorPage ?? InvestorPageActive.InvestorPageSubText,
                ReportTypes = GetActiveReports(),
                InvestorPageColorSubText = preview.ColorSubTextInvestorPage ?? InvestorPageActive.InvestorPageColorSubText,
            };

            return InvestorPagePreview;
        }

        private async Task<InvestorPage> GetActiveInvestorPage()
        {
            var h = await db.InvestorPages.FirstOrDefaultAsync(p => p.InvestorPageActive);
            if (h != null)
            {
                h.ReportTypes = GetActiveReports();
                return h;
            }
            return null;
        }

        private List<ReportType> GetActiveReports()
        {
            return db.ReportTypes.Where(x => x.ReportActive).ToList();
        }

        private static void FillDataTextInvestorPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as InvestorPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "HeadTextInvestorPage":
                        preview.HeadTextInvestorPage = formValue;
                        break;
                    case "ColorHeadTextInvestorPage":
                        preview.ColorHeadTextInvestorPage = formValue;
                        break;
                    case "ColorHeadBgInvestorPage":
                        preview.ColorHeadBgInvestorPage = formValue;
                        break;
                    case "SubTextInvestorPage":
                        preview.SubTextInvestorPage = formValue;
                        break;
                    case "ColorSubTextInvestorPage":
                        preview.ColorSubTextInvestorPage = formValue;
                        break;
                }
            }
        }
    }
}