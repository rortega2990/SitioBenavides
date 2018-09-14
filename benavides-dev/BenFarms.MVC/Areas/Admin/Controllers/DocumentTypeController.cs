using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using System.IO;
using System;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DocumentTypeController : Controller
    {
        private MyApplicationDbContext db = new MyApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.DocumentTypes.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DocumentTypeId,DocumentName,DocumentDescription,DocumentActive")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                db.DocumentTypes.Add(documentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(documentType);
        }

        public async Task<ActionResult> Upload()
        {
            return View(await db.DocumentTypes.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(string v, string p, int? id)
        {
            try
            {
                return await PrepareDataDocumentTypes(FileType.All);
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
            var files = await db.DocumentFiles.Where(x => x.DocumentTypeId == id).ToListAsync();
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType != null)
            {
                ViewBag.TipoDocumento = documentType.DocumentName;
            }

            return View(files);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DocumentTypeId,DocumentName,DocumentDescription,DocumentActive")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(documentType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DocumentType documentType = await db.DocumentTypes.FindAsync(id);
            db.DocumentTypes.Remove(documentType);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteFiles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentFiles documentfiles = await db.DocumentFiles.FindAsync(id);
            if (documentfiles == null)
            {
                return HttpNotFound();
            }
            return View(documentfiles);
        }

        [HttpPost, ActionName("DeleteFiles")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFilesConfirmed(int id)
        {
            DocumentFiles documentfiles = await db.DocumentFiles.FindAsync(id);
            db.DocumentFiles.Remove(documentfiles);
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

        private async Task<JsonResult> PrepareDataDocumentTypes(FileType all)
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            string documentoId = "", textDesc = "", archivoDocumento = "";
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
                        var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ProviderDocuments"), fil);
                        if (!postedFile.FileName.StartsWith("/"))
                        {
                            postedFile.SaveAs(fileSavePath);
                            seSubio = true;
                            switch (file)
                            {
                                case "ArchivoDocumento":
                                    archivoDocumento = $"{"~/ProviderDocuments"}/{fil}";
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
                    case "TipoDocumento":
                        documentoId = formValue;
                        break;
                    case "textDescArchivo":
                        textDesc = formValue;
                        break;
                }
            }

            if (seSubio)
            {
                if (documentoId != null)
                {
                    int id = int.Parse(documentoId);
                    db.DocumentFiles.Add(new DocumentFiles
                    {
                        AddressFile = archivoDocumento,
                        NameDescriptiveFile = textDesc,
                        DocumentTypeId = id

                    });
                }
                await db.SaveChangesAsync();

                return Json(new AjaxResponse { Success = true, Message = "Los datos fueron subidos correctamente al servidor" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
        }
    }
}
