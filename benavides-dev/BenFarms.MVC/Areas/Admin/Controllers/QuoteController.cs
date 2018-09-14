using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Services;
using BenFarms.MVC.Areas.Admin.Models;
using System.Linq;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuoteController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Quotes.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(string v, string p, int? id)
        {
            try
            {
                return await PrepareDataFiles(FileType.Image);
            }
            catch (Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<JsonResult> PrepareDataFiles(FileType all)
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            string quoteText = "", quoteAuthor = "", quoteAuthorSign = "", quoteImage = "";
            bool seSubio = false;
            int quoteId = 0;
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var valid = Utils.IsValidImage(postedFile, all);

                if (valid.Value == "Ok")
                {
                    if (postedFile != null)
                    {
                        var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                        var fileSavePath = Path.Combine( System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), fil);
                        if (!postedFile.FileName.StartsWith("/"))
                        {
                            postedFile.SaveAs(fileSavePath);
                            seSubio = true;
                            switch (file)
                            {
                                case "QuoteAuthorPhoto":
                                    quoteImage = $"{"~/UploadedFiles"}/{fil}";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    return Json(new AjaxResponse { Success = false, Message = valid.Value }, JsonRequestBehavior.AllowGet);
                }
            }

            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "QuoteText":
                        quoteText = formValue;
                        break;
                   case "QuoteAuthor":
                        quoteAuthor = formValue;
                        break;
                    case "QuoteAuthorSign":
                        quoteAuthorSign = formValue;
                        break;
                    case "QuoteId":
                        quoteId = Convert.ToInt32(formValue);
                        break;
                }
            }

            if (seSubio)
            {
                if (!string.IsNullOrEmpty(quoteText) && !string.IsNullOrEmpty(quoteAuthor))
                {
                    if (quoteId == 0)
                    {
                        db.Quotes.Add(new Quote
                        {
                            QuoteAuthorPhoto = quoteImage,
                            QuoteText = quoteText,
                            QuoteAuthor = quoteAuthor, 
                            QuoteAuthorSign = quoteAuthorSign
                        });
                    }
                    else
                    {
                        Quote quote = await db.Quotes.FindAsync(quoteId);
                        if (quote != null)
                        {
                            quote.QuoteAuthorPhoto = quoteImage;
                            quote.QuoteText = quoteText;
                            quote.QuoteAuthor = quoteAuthor;
                            quote.QuoteAuthorSign = quoteAuthorSign;
                            db.Entry(quote).State = EntityState.Modified;
                        }
                    }
                    await db.SaveChangesAsync();
                }

                return Json(new AjaxResponse { Success = true, Message = "Los datos fueron subidos correctamente al servidor" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
        }
        
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            db.Quotes.Remove(quote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AddErrorsToModelState(AdministrationServiceResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }
    }
}
