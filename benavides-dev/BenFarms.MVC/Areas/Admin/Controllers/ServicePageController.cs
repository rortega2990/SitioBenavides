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
    public class ServicePageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var ServicePages = db.ServicePages;
            return View(await ServicePages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var ServicePage = await db.ServicePages.FindAsync(id);

            if (ServicePage != null)
            {
                var ServicePageActive = await GetActiveServicePage();
                if (ServicePageActive != null)
                {
                    if (!ServicePage.ServicePageActive)
                    {
                        ServicePage.ServicePageActive = true;
                        ServicePageActive.ServicePageActive = false;
                        db.Entry(ServicePage).State = EntityState.Modified;
                        db.Entry(ServicePageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return View("Index", await db.ServicePages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            ServicePage ServicePage;
            if (id == null)
            {
                ServicePage = await GetActiveServicePage();
                if (ServicePage != null)
                    return View(ServicePage);
            }
            ServicePage = await db.ServicePages.FindAsync(id);
            if (ServicePage != null)
            {
                ServicePage.ServiceTypes = await GetActiveServiceTypes();
                return View(ServicePage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewService = await db.PagePreviews.FindAsync("PreviewService");

            var str = new MemoryStream(previewService.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ServicePagePreview;

            var ServicePageActive = await GetActiveServicePage();
            var lastIdServicePage = await db.ServicePages.MaxAsync(x => x.ServicePageId) + 1;

            if (preview == null)
            {
                if (ServicePageActive != null)
                    return View("Preview", ServicePageActive);
            }
            else
            {
                var ServicePagePreview = await GetServicePagePreview(ServicePageActive, preview, lastIdServicePage);
                return View("Preview", ServicePagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ServicePage", "~/UploadedFiles", "PreviewService", new ServicePagePreview { ServicePageName = "PreviewService" }, FileType.Image, FillDataTextServicePage, FillDataImageServicePage);

            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var ServicePageActive = await GetActiveServicePage();
            return View(ServicePageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("ServicePage", "~/Content/rsc/imgs", "PreviewService", new ServicePagePreview { ServicePageName = "PreviewService" }, FileType.Image, FillDataTextServicePage, FillDataImageServicePage);

            if (result.Key)
            {
                PagePreview previewService = await db.PagePreviews.FindAsync("PreviewService");

                var str = new MemoryStream(previewService.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as ServicePagePreview;

                if (preview != null)
                {
                    var ServicePageActive = await GetActiveServicePage();
                    var lastIdServicePage = await db.ServicePages.MaxAsync(x => x.ServicePageId) + 1;
                    var ServicePageEdit = await GetServicePagePreview(ServicePageActive, preview, lastIdServicePage);
                    db.ServicePages.Add(ServicePageEdit);
                    await db.SaveChangesAsync();
                    ServicePageActive.ServicePageActive = false;
                    db.Entry(ServicePageActive).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile(string v, string p, int? id)
        {
            try
            {
                return await PrepareDataServiceFiles(FileType.All);
            }
            catch (Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConfirmDeletion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var page = db.ServicePages.Where(p => p.ServicePageId == id).FirstOrDefault();

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.ServicePageCreatedDate,
                Id = page.ServicePageId,
                Active = page.ServicePageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.ServicePageCustomValue,
                Title = "Eliminar página de servicios"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.ServicePages.Where(p => p.ServicePageId == id).FirstOrDefault();
            if (page != null && page.ServicePageActive == false)
            {
                db.ServicePages.Remove(page);
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

        private async Task<ServicePage> GetServicePagePreview(ServicePage ServicePageActive, ServicePagePreview preview, int lastIdServicePage)
        {
            var ServicePagePreview = new ServicePage
            {
                ServicePageCustomValue = $"ServicePage_{lastIdServicePage}",
                ServicePageTitle = "Servicios",
                ServicePageActive = true,
                ServicePageCreatedDate = DateTime.Now,
                ServicePageHeadText1 = preview.Texto1Servicio ?? ServicePageActive.ServicePageHeadText1,
                ServicePageHeadSubText1 = preview.Texto2Servicio ?? ServicePageActive.ServicePageHeadSubText1,
                ServicePageColorHeadText1 = preview.ColorTexto1Servicio ?? ServicePageActive.ServicePageColorHeadText1,
                ServicePageColorHeadSubText1 = preview.ColorTexto2Servicio ?? ServicePageActive.ServicePageColorHeadSubText1,
                ServicePageColorHeadBg = preview.ColorFondoSerivicio ?? ServicePageActive.ServicePageColorHeadBg,
                ServicePageImageLogo = preview.ImagenEncabezadoLogoServicio ?? ServicePageActive.ServicePageImageLogo,
                ServicePageSubText = preview.TextoTituloServicio ?? ServicePageActive.ServicePageSubText,
                ServicePageSubTextDescription = preview.TextDescripcionServicio ?? ServicePageActive.ServicePageSubTextDescription,
                ServicePageColorSubText = preview.ColorTextoTituloServicio ?? ServicePageActive.ServicePageColorSubText,
                ServicePageColorSubTextDescription = preview.ColorTextDescripcionServicio ?? ServicePageActive.ServicePageColorSubTextDescription,
                ServiceTypes = await GetActiveServiceTypes()
            };                        

            return ServicePagePreview;
        }

        private async Task<ServicePage> GetActiveServicePage()
        {
            var h = await db.ServicePages.FirstOrDefaultAsync(p => p.ServicePageActive);
            if (h != null)
            {
                h.ServiceTypes = await GetActiveServiceTypes();
                return h;
            }
            return null;
        }

        private async Task<List<ServiceType>> GetActiveServiceTypes()
        {
            return await db.ServiceTypes.Where(x => x.ServiceTypeActive).ToListAsync();
        }

        private static void FillDataImageServicePage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as ServicePagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImagenEncabezadoLogoServicio":
                        preview.ImagenEncabezadoLogoServicio = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextServicePage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as ServicePagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "Texto1Servicio":
                        preview.Texto1Servicio = formValue;
                        break;
                    case "Texto2Servicio":
                        preview.Texto2Servicio = formValue;
                        break;
                    case "ColorTexto1Servicio":
                        preview.ColorTexto1Servicio = formValue;
                        break;
                    case "ColorTexto2Servicio":
                        preview.ColorTexto2Servicio = formValue;
                        break;
                    case "ColorFondoSerivicio":
                        preview.ColorFondoSerivicio = formValue;
                        break;
                    case "TextoTituloServicio":
                        preview.TextoTituloServicio = formValue;
                        break;
                    case "TextDescripcionServicio":
                        preview.TextDescripcionServicio = formValue;
                        break;
                    case "ColorTextDescripcionServicio":
                        preview.ColorTextDescripcionServicio = formValue;
                        break;
                    case "ColorTextoTituloServicio":
                        preview.ColorTextoTituloServicio = formValue;
                        break;
                }
            }
        }

        private async Task<JsonResult> PrepareDataServiceFiles(FileType all)
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            string documentoId = "", servTypeNameDesc = "", servTypeProductsDesc = "", servTypeActive = "";
            List<ImageUpload> archivoDocumentos = new List<ImageUpload>();
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var valid = Utils.IsValidImage(postedFile, all);

                if (valid.Value == "Ok")
                {
                    if (postedFile != null)
                    {
                        var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                        var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/rsc/imgs"), fil);
                        if (!postedFile.FileName.StartsWith("/"))
                        {
                            postedFile.SaveAs(fileSavePath);
                            ImageUpload im = new ImageUpload
                            {
                                FileName = $"{"~/Content/rsc/imgs"}/{fil}",
                                IsUploaded = true
                            };
                            archivoDocumentos.Add(im);
                        }
                        else
                        {
                            ImageUpload im = new ImageUpload
                            {
                                FileName = $"{"~/Content/rsc/imgs"}/{fil}",
                                IsUploaded = false
                            };
                            archivoDocumentos.Add(im);
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
                    case "ServiceTypeName":
                        documentoId = formValue;
                        break;
                    case "ServiceTypeNameDescription":
                        servTypeNameDesc = formValue;
                        break;
                    case "ServiceTypeProdutcsDescription":
                        servTypeProductsDesc = formValue;
                        break;
                    case "ServiceTypeActive":
                        servTypeActive = formValue;
                        break;
                }
            }

            if (archivoDocumentos.All(x => x.IsUploaded))
            {
                var st = new ServiceType
                {
                    ServiceTypeName = documentoId,
                    ServiceTypeActive = servTypeActive == "true",
                    ServiceTypeNameDescription = servTypeNameDesc,
                    ServiceTypeProdutcsDescription = servTypeProductsDesc,
                    Products = new List<Product>()
                };

                var lastProductId = await db.Products.MaxAsync(x => x.ProductId) + 1;
                int i = 0;
                foreach (var archivoDocumento in archivoDocumentos)
                {
                    i++;
                    st.Products.Add(new Product
                    {
                        ProductName = $"Product_{lastProductId++}",
                        ProductCustomValue = $"Product_ServiceType_{lastProductId}",
                        ProductImage = archivoDocumento.FileName,
                        ProductOrder = i,
                        ProductURL = ""
                    });
                }
                db.ServiceTypes.Add(st);
                await db.SaveChangesAsync();

                return Json(new AjaxResponse { Success = true, Message = "Los datos fueron subidos correctamente al servidor" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = "Ocurrió un error al subir lo(s) archivo(s)." }, JsonRequestBehavior.AllowGet);
        }
    }
}