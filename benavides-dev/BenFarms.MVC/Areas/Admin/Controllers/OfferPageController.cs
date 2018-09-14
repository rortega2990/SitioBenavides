using System;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfferPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var OfferPages = db.OfferPages.Include(x=>x.OfferType);
            return View(await OfferPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var OfferPage = await db.OfferPages.FindAsync(id);

            if (OfferPage != null)
            {
                var OfferPageActive = await GetActiveOfferPage(OfferPage.OfferTypeId);
                if (OfferPageActive != null)
                {
                    if (!OfferPage.OfferPageActive)
                    {
                        OfferPage.OfferPageActive = true;
                        OfferPageActive.OfferPageActive = false;
                        db.Entry(OfferPage).State = EntityState.Modified;
                        db.Entry(OfferPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        var OfferPages = db.OfferPages.Include(x => x.OfferType);
                        return View("Index", await OfferPages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var OfferPage = await db.OfferPages.FindAsync(id);
            if (OfferPage != null)
            {
                OfferPage.OfferTypes = await db.OfferTypes.ToListAsync();
                return View(OfferPage);
            }
            return RedirectToAction("NotFound", "Error");
        }        

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewOffer = await db.PagePreviews.FindAsync("PreviewOffer");

            var str = new MemoryStream(previewOffer.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as OfferPagePreview; 
            
            var lastIdOfferPage = await db.OfferPages.MaxAsync(x => x.OfferPageId) + 1;
            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var OfferPageActive = await GetActiveOfferPage(int.Parse(preview.TipoOferta));
            var OfferPagePreview = GetOfferPagePreview(OfferPageActive, preview, lastIdOfferPage);
            return View("Preview", OfferPagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("OfferPage", "~/UploadedFiles", "PreviewOffer", new OfferPagePreview { OfferPageName = "PreviewOffer" }, FileType.ImagePdf, FillDataTextOfferPage, FillDataImageOfferPage);

            if(result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            OfferPage OfferPageActive;
            if (id == null)
            {
                OfferPageActive = await GetActiveOfferPage(1);
                return View(OfferPageActive);
            }
            OfferPageActive = await GetActiveOfferPage(id.Value);
            if (OfferPageActive != null)
            {
                return View(OfferPageActive);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> EditAjax(int id)
        {
            //var response = new AjaxResponse { Success = false };

            //try
            //{
            //    OfferPage OfferPageActive = await GetActiveOfferPage(id);
            //    response.Message = "Success";
            //    response.Success = true;
            //    response.Data = convertToOfferAjax(OfferPageActive);
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{
            //    response.Message = "Ha ocurrido un error interno en el servidor";
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //} 
            var response = new AjaxResponse { Success = false, Message = "No existe la categoría" };

            try
            {
                OfferPage OfferPageActive = await GetActiveOfferPage(id);
                if (OfferPageActive != null)
                {
                    response.Message = "Success";
                    response.Success = true;
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error interno en el servidor";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private OfferPageAjax convertToOfferAjax(OfferPage offerPage)
        {
            OfferPageAjax o = new OfferPageAjax
            {
                ImagenProductosOfertas = offerPage.OfferImage.Substring(1),
                TextoOfertas1 = offerPage.OfferPageText1,
                TextoOfertas2 = offerPage.OfferPageText2,
                TextoOfertas3= offerPage.OfferPageText3,

                ColorTextoOfertas1= offerPage.OfferPageColorText1,
                ColorTextoOfertas2= offerPage.OfferPageColorText2,
                ColorTextoOfertas3= offerPage.OfferPageColorText3,

                TipoTexto1= offerPage.OfferPageTextType1,
                TipoTexto2= offerPage.OfferPageTextType2,
                TipoTexto3= offerPage.OfferPageTextType3,

                TextoResaltadoOfertas1= offerPage.OfferPageSpan1,
                TextoResaltadoOfertas2= offerPage.OfferPageSpan2,
                TextoResaltadoOfertas3= offerPage.OfferPageSpan3,

                ColorTextoResaltadoOfertas1= offerPage.OfferPageColorSpan1,
                ColorTextoResaltadoOfertas2= offerPage.OfferPageColorSpan2,
                ColorTextoResaltadoOfertas3= offerPage.OfferPageColorSpan3,

                ColorFondoOfertas= offerPage.OfferPageFillColor,
            };


            return o;
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("OfferPage", "~/Content/rsc/imgs", "PreviewOffer", new OfferPagePreview { OfferPageName = "PreviewOffer" }, FileType.ImagePdf, FillDataTextOfferPage, FillDataImageOfferPage);

            if (result.Key)
            {
                PagePreview previewOffer = await db.PagePreviews.FindAsync("PreviewOffer");

                var str = new MemoryStream(previewOffer.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as OfferPagePreview;

                if (preview != null)
                {
                    var OfferPageActive = await GetActiveOfferPage(int.Parse(preview.TipoOferta));
                    var lastIdOfferPage = await db.OfferPages.MaxAsync(x => x.OfferPageId) + 1;
                    var OfferPageEdit = GetOfferPagePreview(OfferPageActive, preview, lastIdOfferPage);
                    db.OfferPages.Add(OfferPageEdit);
                    await db.SaveChangesAsync();
                    OfferPageActive.OfferPageActive = false;
                    db.Entry(OfferPageActive).State = EntityState.Modified;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private OfferPage GetOfferPagePreview(OfferPage OfferPageActive, OfferPagePreview preview, int lastIdOfferPage)
        {
            var idtipoOferta = int.Parse(preview.TipoOferta);
            var tipoOferta = db.OfferTypes.FirstOrDefault(x=>x.OfferTypeId == idtipoOferta);
            if (tipoOferta != null)
            {
                var offerPagePreview = new OfferPage
                {
                    OfferPageCustomValue = $"OfferPage_{tipoOferta.OfferTypeName.Trim()}_{lastIdOfferPage}",
                    OfferPageTitle = "Ofertas",
                    OfferPageActive = true,
                    OfferPageCreatedDate = DateTime.Now,
                    OfferImage = preview.ImagenProductosOfertas ?? OfferPageActive.OfferImage,
                    OfferTypeId = idtipoOferta,
                    OfferPageFillColor = preview.ColorFondoOfertas ?? OfferPageActive.OfferPageFillColor,
                    OfferPageText1 = preview.TextoOfertas1 ?? OfferPageActive.OfferPageText1,
                    OfferPageColorText1 = preview.ColorTextoOfertas1 ?? OfferPageActive.OfferPageColorText1,
                    OfferPageSpan1 = preview.TextoResaltadoOfertas1 ?? OfferPageActive.OfferPageSpan1,
                    OfferPageColorSpan1 = preview.ColorTextoResaltadoOfertas1 ?? OfferPageActive.OfferPageColorSpan1,
                    OfferPageTextType1 =
                        string.IsNullOrEmpty(preview.TipoTexto1) || preview.TipoTexto1 == "em"
                            ? OfferPageActive.OfferPageTextType1
                            : preview.TipoTexto1,
                    OfferPageText2 = preview.TextoOfertas2 ?? OfferPageActive.OfferPageText2,
                    OfferPageColorText2 = preview.ColorTextoOfertas2 ?? OfferPageActive.OfferPageColorText2,
                    OfferPageSpan2 = preview.TextoResaltadoOfertas2 ?? OfferPageActive.OfferPageSpan2,
                    OfferPageColorSpan2 = preview.ColorTextoResaltadoOfertas2 ?? OfferPageActive.OfferPageColorSpan2,
                    OfferPageTextType2 =
                        string.IsNullOrEmpty(preview.TipoTexto2) || preview.TipoTexto2 == "em"
                            ? OfferPageActive.OfferPageTextType2
                            : preview.TipoTexto2,
                    OfferPageText3 = preview.TextoOfertas3 ?? OfferPageActive.OfferPageText3,
                    OfferPageColorText3 = preview.ColorTextoOfertas3 ?? OfferPageActive.OfferPageColorText3,
                    OfferPageSpan3 = preview.TextoResaltadoOfertas3 ?? OfferPageActive.OfferPageSpan3,
                    OfferPageColorSpan3 = preview.ColorTextoResaltadoOfertas3 ?? OfferPageActive.OfferPageColorSpan3,
                    OfferPageTextType3 =
                        string.IsNullOrEmpty(preview.TipoTexto3) || preview.TipoTexto3 == "em"
                            ? OfferPageActive.OfferPageTextType3
                            : preview.TipoTexto3,
                    OfferTypes = db.OfferTypes.ToList(),
                };
                return offerPagePreview;
            }
            return null;
        }

        private async Task<OfferPage> GetActiveOfferPage(int type)
        {
            var h = await db.OfferPages.FirstOrDefaultAsync(p => p.OfferPageActive && p.OfferTypeId == type);
            if (h != null)
            {
                h.OfferTypes = await db.OfferTypes.ToListAsync();
                return h;
            }
            return null;
        }

        private static void FillDataImageOfferPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as OfferPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImagenProductosOfertas":
                        preview.ImagenProductosOfertas = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextOfferPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as OfferPagePreview;
            var formValue = httpRequest.Form[form];
            formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
            if (preview != null)
            {
                switch (form)
                {
                    case "TipoOferta":
                        preview.TipoOferta = formValue;
                        break;
                    case "ColorFondoOfertas":
                        preview.ColorFondoOfertas = formValue;
                        break;
                    case "TextoOfertas1":
                        preview.TextoOfertas1 = formValue;
                        break;
                    case "TextoOfertas2":
                        preview.TextoOfertas2 = formValue;
                        break;
                    case "TextoOfertas3":
                        preview.TextoOfertas3 = formValue;
                        break;
                    case "ColorTextoOfertas1":
                        preview.ColorTextoOfertas1 = formValue;
                        break;
                    case "ColorTextoOfertas2":
                        preview.ColorTextoOfertas2 = formValue;
                        break;
                    case "ColorTextoOfertas3":
                        preview.ColorTextoOfertas3 = formValue;
                        break;
                    case "TipoTexto1":
                        preview.TipoTexto1 = formValue;
                        break;
                    case "TipoTexto2":
                        preview.TipoTexto2 = formValue;
                        break;
                    case "TipoTexto3":
                        preview.TipoTexto3 = formValue;
                        break;
                    case "TextoResaltadoOfertas1":
                        preview.TextoResaltadoOfertas1 = formValue;
                        break;
                    case "TextoResaltadoOfertas2":
                        preview.TextoResaltadoOfertas2 = formValue;
                        break;
                    case "TextoResaltadoOfertas3":
                        preview.TextoResaltadoOfertas3 = formValue;
                        break;
                    case "ColorTextoResaltadoOfertas1":
                        preview.ColorTextoResaltadoOfertas1 = formValue;
                        break;
                    case "ColorTextoResaltadoOfertas2":
                        preview.ColorTextoResaltadoOfertas2 = formValue;
                        break;
                    case "ColorTextoResaltadoOfertas3":
                        preview.ColorTextoResaltadoOfertas3 = formValue;
                        break;
                }
            }
        }
    }

    internal class OfferPageAjax
    {
        public string ImagenProductosOfertas { get; set; }

        public string TextoOfertas1 { get; set; }
        public string TextoOfertas2 { get; set; }
        public string TextoOfertas3 { get; set; }

        public string ColorTextoOfertas1 { get; set; }
        public string ColorTextoOfertas2 { get; set; }
        public string ColorTextoOfertas3 { get; set; }

        public string TipoTexto1 { get; set; }
        public string TipoTexto2 { get; set; }
        public string TipoTexto3 { get; set; }

        public string TextoResaltadoOfertas1 { get; set; }
        public string TextoResaltadoOfertas2 { get; set; }
        public string TextoResaltadoOfertas3 { get; set; }

        public string ColorTextoResaltadoOfertas1 { get; set; }
        public string ColorTextoResaltadoOfertas2 { get; set; }
        public string ColorTextoResaltadoOfertas3 { get; set; }

        public string TipoOferta { get; set; }
        public string ColorFondoOfertas { get; set; }
    }
}
