using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using BenFarms.MVC.Models;
using System.Web;
using System.Net;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JoinTeamPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var JoinTeamPages = db.JoinTeamPages;
            return View(await JoinTeamPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var JoinTeamPage = await db.JoinTeamPages.FindAsync(id);

            if (JoinTeamPage != null)
            {
                var JoinTeamPageActive = await GetActiveJoinTeamPage();
                if (JoinTeamPageActive != null)
                {
                    if (!JoinTeamPage.JoinTeamPageActive)
                    {
                        JoinTeamPage.JoinTeamPageActive = true;
                        JoinTeamPageActive.JoinTeamPageActive = false;
                        db.Entry(JoinTeamPage).State = EntityState.Modified;
                        db.Entry(JoinTeamPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.JoinTeamPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> JoinedUsersList()
        {
            var UserJoined = db.UserJoinedToTeams;
            return View(await UserJoined.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            JoinTeamPage JoinTeamPage;
            if (id == null)
            {
                JoinTeamPage = await GetActiveJoinTeamPage();
                if (JoinTeamPage != null)
                    return View(JoinTeamPage);
            }
            JoinTeamPage = await db.JoinTeamPages.FindAsync(id);
            if (JoinTeamPage != null)
            {
                JoinTeamPage.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == JoinTeamPage.JoinTeamPageId && x.ImageSectionPageName == "JoinTeamPage").ToListAsync();
                return View(JoinTeamPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewJoinTeam = await db.PagePreviews.FindAsync("PreviewJoinTeam");

            var str = new MemoryStream(previewJoinTeam.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as JoinTeamPagePreview;

            var JoinTeamPageActive = await GetActiveJoinTeamPage();
            var lastIdJoinTeamPage = await db.JoinTeamPages.MaxAsync(x => x.JoinTeamPageId) + 1;

            if (preview == null)
            {
                if (JoinTeamPageActive != null)
                    return View("Preview", JoinTeamPageActive);
            }
            else
            {
                var JoinTeamPagePreview = GetJoinTeamPagePreview(JoinTeamPageActive, preview, lastIdJoinTeamPage);
                return View("Preview", JoinTeamPagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("JoinTeamPage", "~/UploadedFiles", "PreviewJoinTeam", new JoinTeamPagePreview { JoinTeamPageName = "PreviewJoinTeam" }, FileType.Image, FillDataTextJoinTeamPage, null);
            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var team = await GetActiveJoinTeamPage();
            return View(team);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("JoinTeamPage", "~/UploadedFiles", "PreviewJoinTeam", new JoinTeamPagePreview { JoinTeamPageName = "PreviewJoinTeam" }, FileType.Image, FillDataTextJoinTeamPage, null);

            if (result.Key)
            {
                PagePreview previewJoinTeam = await db.PagePreviews.FindAsync("PreviewJoinTeam");

            var str = new MemoryStream(previewJoinTeam.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as JoinTeamPagePreview;

            if (preview != null)
            {
                var JoinTeamPageActive = await GetActiveJoinTeamPage();
                var lastIdJoinTeamPage = await db.JoinTeamPages.MaxAsync(x => x.JoinTeamPageId) + 1;
                var JoinTeamPageEdit = GetJoinTeamPagePreview(JoinTeamPageActive, preview, lastIdJoinTeamPage);
                    var id = db.JoinTeamPages.Add(JoinTeamPageEdit);
                    await db.SaveChangesAsync();
                    foreach (var h in JoinTeamPageEdit.HeadImages)
                    {
                        h.ImageSectionPageId = id.JoinTeamPageId;
                        db.ImageSections.Add(h);
                    }
                    await db.SaveChangesAsync();
                    JoinTeamPageActive.JoinTeamPageActive = false;
                    db.Entry(JoinTeamPageActive).State = EntityState.Modified;
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

            var page = db.JoinTeamPages.Where(p => p.JoinTeamPageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.JoinTeamPageCreatedDate,
                Id = page.JoinTeamPageId,
                Active = page.JoinTeamPageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.JoinTeamPageCustomValue,
                Title = "Eliminar página de únete al equipo"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.JoinTeamPages.Where(p => p.JoinTeamPageId == id).FirstOrDefault();
            if (page != null && page.JoinTeamPageActive == false)
            {
                db.JoinTeamPages.Remove(page);
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

        public async Task<ActionResult> DetailsJoinedUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserJoinedToTeam userJoinedToTeam = await db.UserJoinedToTeams.FindAsync(id);
            if (userJoinedToTeam == null)
            {
                return HttpNotFound();
            }
            return View(userJoinedToTeam);
        }

        public async Task<ActionResult> DeleteJoinedUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserJoinedToTeam userJoinedToTeam = await db.UserJoinedToTeams.FindAsync(id);
            if (userJoinedToTeam == null)
            {
                return HttpNotFound();
            }
            return View(userJoinedToTeam);
        }

        [HttpPost, ActionName("DeleteJoinedUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteJoinedUserConfirmed(int id)
        {
            UserJoinedToTeam userJoinedToTeam = await db.UserJoinedToTeams.FindAsync(id);
            db.UserJoinedToTeams.Remove(userJoinedToTeam);
            await db.SaveChangesAsync();
            return RedirectToAction("JoinedUsersList");
        }

        private JoinTeamPage GetJoinTeamPagePreview(JoinTeamPage JoinTeamPageActive, JoinTeamPagePreview preview, int lastIdJoinTeamPage)
        {
            var JoinTeamPagePreview = new JoinTeamPage
            {
                JoinTeamPageCustomValue = $"JoinTeamPage_{lastIdJoinTeamPage}",
                JoinTeamPageTitle = "Únete al equipo",
                JoinTeamPageActive = true,
                JoinTeamPageCreatedDate = DateTime.Now,
                HeadImages = Utils.ConvertToImageSectionList(preview.Encabezado),
                JoinTeamPageSubText1 = preview.SubTextJoinTeamPage1 ?? JoinTeamPageActive.JoinTeamPageSubText1,
                JoinTeamPageSubText2 = preview.SubTextJoinTeamPage2 ?? JoinTeamPageActive.JoinTeamPageSubText2,
                JoinTeamPageColorText1 = preview.ColorText1JoinTeamPage ?? JoinTeamPageActive.JoinTeamPageColorText1,
                JoinTeamPageColorText2 = preview.ColorText2JoinTeamPage ?? JoinTeamPageActive.JoinTeamPageColorText2,
            };                        

            return JoinTeamPagePreview;
        }

        private async Task<JoinTeamPage> GetActiveJoinTeamPage()
        {
            var h = await db.JoinTeamPages.FirstOrDefaultAsync(p => p.JoinTeamPageActive);
            if (h != null)
            {
                h.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == h.JoinTeamPageId && x.ImageSectionPageName == "JoinTeamPage").ToListAsync();
                return h;
            }
            return null;
        }

        private static void FillDataTextJoinTeamPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as JoinTeamPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "SubTextJoinTeamPage1":
                        preview.SubTextJoinTeamPage1 = formValue;
                        break;
                    case "SubTextJoinTeamPage2":
                        preview.SubTextJoinTeamPage2 = formValue;
                        break;
                    case "ColorText1JoinTeamPage":
                        preview.ColorText1JoinTeamPage = formValue;
                        break;
                    case "ColorText2JoinTeamPage":
                        preview.ColorText2JoinTeamPage = formValue;
                        break;
                }
            }
        }
    }
}