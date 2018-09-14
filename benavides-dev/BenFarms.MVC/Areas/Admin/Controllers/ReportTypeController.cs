using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using System.IO;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportTypeController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.ReportTypes.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReportTypeId,ReportName,ReportDescription,ReportActive")] ReportType reportType)
        {
            if (ModelState.IsValid)
            {
                db.ReportTypes.Add(reportType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(reportType);
        }

        public async Task<ActionResult> Upload()
        {
            return View(await db.ReportTypes.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(string v, string p, int? id)
        {            
            try
            {
                return await PrepareDataInvestorFiles(FileType.All);
            }
            catch (Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Files(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var files = await db.ReportFiles.Where(x => x.ReportTypeId == id).OrderBy(y => y.Year).ToListAsync();
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            if (reportType != null)
            {
                ViewBag.TipoReporte = reportType.ReportName;
            }
            
            return View(files);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            if (reportType == null)
            {
                return HttpNotFound();
            }
            return View(reportType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReportTypeId,ReportName,ReportDescription,ReportActive")] ReportType reportType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reportType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            if (reportType == null)
            {
                return HttpNotFound();
            }
            return View(reportType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReportType reportType = await db.ReportTypes.FindAsync(id);
            db.ReportTypes.Remove(reportType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteFiles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportFiles reportFiles = await db.ReportFiles.FindAsync(id);
            if (reportFiles == null)
            {
                return HttpNotFound();
            }
            return View(reportFiles);
        }

        [HttpPost, ActionName("DeleteFiles")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFilesConfirmed(int id)
        {
            ReportFiles reportFiles = await db.ReportFiles.FindAsync(id);
            db.ReportFiles.Remove(reportFiles);
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

        private async Task<JsonResult> PrepareDataInvestorFiles(FileType all)
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            string reporteId = "", yearSel = "", archivoReporte = "", descrip = "";
            bool seSubio = false;
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var valid = Utils.IsValidImage(postedFile, all);

                if (valid.Value == "Ok")
                {
                    if (postedFile != null)
                    {
                        var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                        var fileSavePath = Path.Combine( System.Web.HttpContext.Current.Server.MapPath("~/Reports"), fil);
                        if (!postedFile.FileName.StartsWith("/"))
                        {
                            postedFile.SaveAs(fileSavePath);
                            seSubio = true;
                            switch (file)
                            {
                                case "ArchivoReporte":
                                    archivoReporte = $"{"~/Reports"}/{fil}";
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
                    case "TipoReporte":
                        reporteId = formValue;
                        break;
                   case "ReporteDescriptionFile":
                        descrip = formValue;
                        break;
                    case "IdYearReporte":
                        yearSel = formValue;
                        break;
                }
            }

            if (seSubio)
            {
                if (reporteId != null && yearSel != null)
                {
                    int id = int.Parse(reporteId);
                    int year = int.Parse(yearSel);

                    db.ReportFiles.Add(new ReportFiles
                        {
                            AddressFile = archivoReporte,
                            Year = year,
                            ReportTypeId = id, 
                            DescriptionFile = descrip
                        });
                        await db.SaveChangesAsync();
                }

                return Json(new AjaxResponse { Success = true, Message = "Los datos fueron subidos correctamente al servidor" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
        }
    }
}
