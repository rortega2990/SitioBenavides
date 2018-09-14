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
using System.Web.Routing;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FosePageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var FosePages = db.FosePages;
            return View(await FosePages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> ListPromocions(int? idFose, int? idPromo, int? v)
        {
            ViewBag.Listar = v;
            ViewBag.Fose = idFose;

            if (idFose != null)
            {
                FosePage fosePage = await db.FosePages.FindAsync(idFose);

                if (fosePage != null)
                {
                    ViewBag.FosePage = fosePage.FosePageCustomValue;
                    return View(fosePage.Promocions);
                }
            }
            if (idPromo != null)
            {
                PromocionPage promocionPage = await db.PromocionPages.FindAsync(idPromo);

                if (promocionPage != null)
                {
                    FosePage fosePage = await db.FosePages.FindAsync(promocionPage.FosePageId);

                    if (fosePage != null)
                    {
                        ViewBag.FosePage = fosePage.FosePageCustomValue;
                        return View(fosePage.Promocions);
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> ListPromocionsProducts(int? idFose, int? idPromo, int? v)
        {
            ViewBag.Listar = v;
            ViewBag.Fose = idFose;
            PromocionPage PromocionPage = await db.PromocionPages.FindAsync(idPromo);
            if (PromocionPage != null)
            {
                ViewBag.PromocionPage = PromocionPage.PromocionPageCustomValue;
                ViewBag.PromocionPageId = PromocionPage.PromocionPageId;
                return View(PromocionPage.ProductPages);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Active(int id)
        {
            FosePage FosePage = await db.FosePages.FindAsync(id);
            if (FosePage != null)
            {
                if (FosePage.FosePageActive == "EnEdicion" || FosePage.FosePageActive == "Desactivada")
                {
                    return View("ActiveConfirmed", FosePage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("BillingPageActive")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveConfirmed(int id)
        {
            FosePage FosePage = await db.FosePages.FindAsync(id);
            if (FosePage != null)
            {
                var FosePageActive = await GetActiveFosePage();
                if (FosePageActive != null)
                {
                    if (FosePage.FosePageActive == "EnEdicion" || FosePage.FosePageActive == "Desactivada")
                    {
                        FosePage.FosePageActive = "Activada";
                        foreach (var promocionPage in FosePage.Promocions)
                        {
                            promocionPage.PromocionPageActive = "Activada";
                            foreach (var productPage in promocionPage.ProductPages)
                            {
                                productPage.ProductPageActive = "Activada";
                            }
                        }

                        FosePageActive.FosePageActive = "Desactivada";
                        foreach (var promocionPage in FosePageActive.Promocions)
                        {
                            promocionPage.PromocionPageActive = "Desactivada";
                            foreach (var productPage in promocionPage.ProductPages)
                            {
                                productPage.ProductPageActive = "Desactivada";
                            }
                        }

                        db.Entry(FosePage).State = EntityState.Modified;
                        db.Entry(FosePageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            FosePage FosePage = await db.FosePages.FindAsync(id);
            if (FosePage != null)
            {
                if (FosePage.FosePageActive == "EnEdicion")
                {
                    return View("DeleteConfirmed", FosePage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var FosePage = await db.FosePages.FindAsync(id);

            if (FosePage != null)
            {
                if (FosePage.FosePageActive == "EnEdicion")
                {
                    db.FosePages.Remove(FosePage);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> DeletePromocion(int? idPromo, int? idFose)
        {
            ViewBag.Fose = idFose;
            PromocionPage PromocionPage = await db.PromocionPages.FindAsync(idPromo);
            if (PromocionPage != null)
            {
                if (PromocionPage.PromocionPageActive == "EnEdicion")
                {
                    return View("DeletePromocionConfirmed", PromocionPage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("DeletePromocion")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePromocionConfirmed(int? idPromo, int? idFose)
        {
            var PromocionPage = await db.PromocionPages.FindAsync(idPromo);            
            if (PromocionPage != null)
            {
                var r = new RouteValueDictionary {{"idFose", PromocionPage.FosePageId}};
                if (PromocionPage.PromocionPageActive == "EnEdicion")
                {
                    db.PromocionPages.Remove(PromocionPage);
                    await db.SaveChangesAsync();                    
                    return RedirectToAction("ListPromocions", r);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> DeletePromocionProduct(int? idProd, int? idPromo)
        {
            ViewBag.Promo = idPromo;
            ProductPage ProductPage = await db.ProductPages.FindAsync(idProd);
            if (ProductPage != null)
            {
                if (ProductPage.ProductPageActive == "EnEdicion")
                {
                    return View("DeletePromocionProductConfirmed", ProductPage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("DeletePromocionProduct")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePromocionProductConfirmed(int? idProd, int? idPromo)
        {
            var ProductPage = await db.ProductPages.FindAsync(idProd);
            if (ProductPage != null)
            {
                var r = new RouteValueDictionary { { "idPromo", ProductPage.PromocionPageId } };
                if (ProductPage.ProductPageActive == "EnEdicion")
                {
                    db.ProductPages.Remove(ProductPage);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListPromocionsProducts", r);
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
            FosePage FosePage = await db.FosePages.FindAsync(id);
            if (FosePage != null)
            {
                FosePage.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == FosePage.FosePageId && x.ImageSectionPageName == "FosePage").ToListAsync();
                return View(FosePage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewPromocion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var PromocionPage = await db.PromocionPages.FindAsync(id);
            if (PromocionPage != null)
            {
                return View(PromocionPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewPromocionProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var ProductPage = await db.ProductPages.FindAsync(id);
            if (ProductPage != null)
            {
                return View(ProductPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewFose = await db.PagePreviews.FindAsync("PreviewFose");

            var str = new MemoryStream(previewFose.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as FosePagePreview;

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var FosePagePreview = await GetFosePagePreview(preview);
            return View("Preview", FosePagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("FosePage", "~/UploadedFiles", "PreviewFose", new FosePagePreview { FosePageName = "PreviewFose" }, FileType.Image, FillDataTextFosePage, null);
            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEditPromocion()
        {
            var previewPromocion = await db.PagePreviews.FindAsync("PreviewPromocion");

            var str = new MemoryStream(previewPromocion.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as PromocionPagePreview;

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var PromocionPagePreview = await GetPromocionPagePreview(preview);
            return View("PreviewPromocion", PromocionPagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEditPromocion(int? idPromo, int? idFose)
        {
            var result = await Utils.PrepareDataPage("PromocionPage", "~/UploadedFiles", "PreviewPromocion", new PromocionPagePreview { PromocionPageName = "PreviewPromocion", PromocionPageId = idPromo, FosePageId = idFose }, FileType.Image, FillDataTextPromocionPage, FillDataImagePromocionPage);
            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEditPromocionProduct()
        {
            var previewProducts = await db.PagePreviews.FindAsync("PreviewProduct");

            var str = new MemoryStream(previewProducts.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as ProductPagePreview;

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var ProductPagePreview = await GetProductPagePreview(preview);
            return View("PreviewPromocionProduct", ProductPagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEditPromocionProduct(int? idProd, int? idPromo)
        {
            var result = await Utils.PrepareDataPage("ProductPage", "~/UploadedFiles", "PreviewProduct", new ProductPagePreview { ProductPageName = "PreviewProduct", ProductPageId = idProd, PromocionPageId = idPromo }, FileType.Image, FillDataTextProductPage, FillDataImageProductPage);
            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var fose = await GetActiveFosePage();
            return View(fose);
        }

        [HttpGet]
        public async Task<ActionResult> EditPromocion(int? idPromo, int? idFose)
        {
            ViewBag.Fose = idFose;
            PromocionPage page = (idPromo != null) ? await db.PromocionPages.FindAsync(idPromo) : new PromocionPage();
            return View(page);
        }

        [HttpGet]
        public async Task<ActionResult> EditPromocionProduct(int? idProd, int? idPromo, int? idFose)
        {
            ViewBag.Fose = idFose;
            ProductPage page;
            if (idProd != null)
            {
                page = await db.ProductPages.FindAsync(idProd.Value);
                return View(page);
            }else
            {
                if(idPromo != null)
                {
                    page = new ProductPage() { PromocionPageId = idPromo.Value };
                    return View(page);
                }
            }

            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("FosePage", "~/Content/rsc/imgs", "PreviewFose", new FosePagePreview { FosePageName = "PreviewFose" }, FileType.Image, FillDataTextFosePage, null);

            if (result.Key)
            {
                PagePreview previewFose = await db.PagePreviews.FindAsync("PreviewFose");

                var str = new MemoryStream(previewFose.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as FosePagePreview;

                if (preview != null)
                {
                    var FosePagePreview = await GetFosePagePreview(preview);

                    var id = db.FosePages.Add(FosePagePreview);
                    await db.SaveChangesAsync();

                    foreach (var fose in FosePagePreview.HeadImages)
                    {
                        fose.ImageSectionPageId = id.FosePageId;
                        db.ImageSections.Add(fose);
                    }
                    await db.SaveChangesAsync();

                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
            //var result = await Utils.PrepareDataPage("FosePage", "~/Content/rsc/imgs", "PreviewFose", new FosePagePreview { FosePageName = "PreviewFose" }, FileType.Image, FillDataTextFosePage, null);

            //if (result.Key)
            //{
            //    PagePreview previewFose = await db.PagePreviews.FindAsync("PreviewFose");

            //    var str = new MemoryStream(previewFose.PageValue);
            //    var binaryFormatter = new BinaryFormatter();
            //    var preview = binaryFormatter.Deserialize(str) as FosePagePreview;

            //    if (preview != null)
            //    {
            //        var FosePageEdit = await GetFosePagePreview(preview);
            //        var lastIdProduct = await db.Products.MaxAsync(x => x.ProductId) + 1;                    
            //        var lastIdProductPage = await db.ProductPages.MaxAsync(x => x.ProductPageId) + 1;

            //        var id = db.FosePages.Add(FosePageEdit);
            //        await db.SaveChangesAsync();

            //        foreach (var fose in FosePageEdit.HeadImages)
            //        {
            //            fose.ImageSectionPageId = id.FosePageId;
            //            db.ImageSections.Add(fose);
            //        }
            //        await db.SaveChangesAsync();


            //        foreach (var promocion in FosePageEdit.Promocions)
            //        {
            //            if(promocion.HeadImage.ImageSectionPageId > 0)
            //            {
            //                PromocionPage prom = await db.PromocionPages.FindAsync(promocion.HeadImage.ImageSectionPageId);
            //                if(prom != null)
            //                {
            //                    var i = 1;
            //                    foreach (var prod in prom.ProductPages)
            //                    {
            //                        var ProductPage = new ProductPage();

            //                        ProductPage.ProductPageActive = "EnEdicion";
            //                        ProductPage.ProductPageCreatedDate = DateTime.Now;
            //                        ProductPage.ProductPageCustomValue = $"ProductPage_{lastIdProductPage}";
            //                        ProductPage.Product = new Product
            //                        {
            //                            ProductName = $"Product_{lastIdProduct++}",
            //                            ProductCustomValue = $"Product_Promocion_{lastIdProductPage++}",
            //                            ProductImage = prod.Product.ProductImage,
            //                            ProductOrder = i++,
            //                            ProductURL = "",
            //                            ProductSubtitle = prod.Product.ProductSubtitle,
            //                        };
            //                        ProductPage.ProductPageBgColor = prod.ProductPageBgColor;
            //                        ProductPage.ProductPageTextTitle = prod.ProductPageTextTitle;
            //                        ProductPage.ProductPageColorTextTitle = prod.ProductPageColorTextTitle;
            //                        ProductPage.ProductPageTextDescription1 = prod.ProductPageTextDescription1;
            //                        ProductPage.ProductPageTextDescription2 = prod.ProductPageTextDescription2;
            //                        ProductPage.ProductPageTextCharacteristic1 = prod.ProductPageTextCharacteristic1;
            //                        ProductPage.ProductPageTextCharacteristic2 = prod.ProductPageTextCharacteristic2;
            //                        ProductPage.ProductPageColorTextDescription1 = prod.ProductPageColorTextDescription1;
            //                        ProductPage.ProductPageColorTextDescription2 = prod.ProductPageColorTextDescription2;
            //                        ProductPage.ProductPageColorTextCharacteristic1 = prod.ProductPageColorTextCharacteristic1;
            //                        ProductPage.ProductPageColorTextCharacteristic2 = prod.ProductPageColorTextCharacteristic2;

            //                        promocion.ProductPages.Add(ProductPage);
            //                    }
            //                    promocion.PromocionPageImageLogo1 = prom.PromocionPageImageLogo1;
            //                    promocion.PromocionPageImageLogo2 = prom.PromocionPageImageLogo2;
            //                    promocion.PromocionPageHeadText = prom.PromocionPageHeadText;
            //                    promocion.PromocionPageSpanHeadText = prom.PromocionPageSpanHeadText;
            //                    promocion.PromocionPageColorHeadBg = prom.PromocionPageColorHeadBg;
            //                    promocion.PromocionPageSpanHeadtextColor = prom.PromocionPageSpanHeadtextColor;
            //                    promocion.PromocionPageHeadtextColor = prom.PromocionPageHeadtextColor;
            //                    promocion.PromocionPageSubText1 = prom.PromocionPageSubText1;
            //                    promocion.PromocionPageSubText2 = prom.PromocionPageSubText2;
            //                }
            //                promocion.PromocionPageActive = "EnEdicion";
            //            }
                        
            //            promocion.HeadImage.ImageSectionPageId = promocion.PromocionPageId;
            //            db.ImageSections.Add(promocion.HeadImage);
            //        }
            //        await db.SaveChangesAsync();

            //        return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyViewPromocion(int? idPromo, int? idFose)
        {
            var result = await Utils.PrepareDataPage("PromocionPage", "~/Content/rsc/imgs", "PreviewPromocion", new PromocionPagePreview { PromocionPageName = "PreviewPromocion", PromocionPageId = idPromo, FosePageId = idFose }, FileType.Image, FillDataTextPromocionPage, FillDataImagePromocionPage);

            if (result.Key)
            {
                PagePreview previewPromocion = await db.PagePreviews.FindAsync("PreviewPromocion");

                var str = new MemoryStream(previewPromocion.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as PromocionPagePreview;

                if (preview != null)
                {
                    var PromocionPageEdit = await GetPromocionPagePreview(preview);
                    if (idPromo == 0)
                    {
                        if (idFose != null) PromocionPageEdit.FosePageId = idFose.Value;
                        db.PromocionPages.Add(PromocionPageEdit);
                    }
                    else
                    {
                        db.Entry(PromocionPageEdit).State = EntityState.Modified;
                    }
                    await db.SaveChangesAsync();
                    
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyViewPromocionProduct(int? idProd, int? idPromo)
        {
            var result = await Utils.PrepareDataPage("ProductPage", "~/Content/rsc/imgs", "PreviewProduct", new ProductPagePreview { ProductPageName = "PreviewProduct", ProductPageId = idProd, PromocionPageId = idPromo }, FileType.Image, FillDataTextProductPage, FillDataImageProductPage);

            if (result.Key)
            {
                PagePreview productPromocion = await db.PagePreviews.FindAsync("PreviewProduct");

                var str = new MemoryStream(productPromocion.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as ProductPagePreview;

                if (preview != null)
                {
                    var page = await GetProductPagePreview(preview);
                    if(preview.ProductPageId == 0)
                    {
                        db.ProductPages.Add(page);
                    }
                    else
                    {
                        db.Entry(page).State = EntityState.Modified;
                    }                   
                    await db.SaveChangesAsync();
                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
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

        private async Task<FosePage> GetFosePagePreview(FosePagePreview preview)
        {
            var FosePageActive = await GetActiveFosePage();
            var lastIdFosePage = await db.FosePages.MaxAsync(x => x.FosePageId) + 1;
            var lastIdPromocion = await db.PromocionPages.MaxAsync(x => x.PromocionPageId);
            var lastIdProductPage = await db.ProductPages.MaxAsync(x => x.ProductPageId);
            var lastIdProduct = await db.Products.MaxAsync(x => x.ProductId);

            var FosePagePreview = new FosePage
            {
                FosePageCustomValue = $"FosePage_{lastIdFosePage}",
                FosePageTitle = "Fose",
                FosePageActive = "EnEdicion",
                FosePageCreatedDate = DateTime.Now,
                FoseTextBranch = preview.TextoSucursalesFose ?? FosePageActive.FoseTextBranch,
                Promocions = new List<PromocionPage>(),
                HeadImages = Utils.ConvertToImageSectionList(preview.Encabezado)
            };

            for(int i = 0; i < FosePageActive.Promocions.Count; i++)
            {
                var promo = FosePageActive.Promocions[i];

                var newPromo = new PromocionPage
                {
                    PromocionPageActive = "EnEdicion",
                    PromocionPageCreatedDate = DateTime.Now,
                    PromocionPageCustomValue = $"Promocion_{++lastIdPromocion}",
                    PromocionPageHeadImage = promo.PromocionPageHeadImage,
                    PromocionPageTextFose= promo.PromocionPageTextFose,
                    PromocionPageTextColorFose = promo.PromocionPageTextColorFose,
                    PromocionPageOrder = i + 1,
                    PromocionPageImageLogo1 = promo.PromocionPageImageLogo1,
                    PromocionPageImageLogo2 = promo.PromocionPageImageLogo2,
                    PromocionPageHeadText = promo.PromocionPageHeadText,
                    PromocionPageSpanHeadText = promo.PromocionPageSpanHeadText,
                    PromocionPageColorHeadBg = promo.PromocionPageColorHeadBg,
                    PromocionPageSpanHeadtextColor = promo.PromocionPageSpanHeadtextColor,
                    PromocionPageHeadtextColor = promo.PromocionPageHeadtextColor,
                    PromocionPageSubText1 = promo.PromocionPageSubText1,
                    PromocionPageSubText2 = promo.PromocionPageSubText2,
                    ProductPages = new List<ProductPage>()
                };

                for (int j = 0; j < promo.ProductPages.Count; j++)
                {
                    var product = promo.ProductPages[j];
                    newPromo.ProductPages.Add(new ProductPage
                    {
                        ProductPageActive = "EnEdicion", 
                        ProductPageCreatedDate = DateTime.Now,
                        ProductPageCustomValue = $"ProductPage_{++lastIdProductPage}",
                        ProductPageBgColor = product.ProductPageBgColor,
                        ProductPageColorTextCharacteristic1 = product.ProductPageColorTextCharacteristic1,
                        ProductPageColorTextCharacteristic2 = product.ProductPageColorTextCharacteristic2,
                        ProductPageColorTextDescription1 = product.ProductPageColorTextDescription1,
                        ProductPageColorTextDescription2 = product.ProductPageColorTextDescription2,
                        ProductPageColorTextTitle = product.ProductPageColorTextTitle,
                        ProductPageTextCharacteristic1 = product.ProductPageTextCharacteristic1,
                        ProductPageTextCharacteristic2 = product.ProductPageTextCharacteristic2,
                        ProductPageTextDescription1 = product.ProductPageTextDescription1,
                        ProductPageTextDescription2 = product.ProductPageTextDescription2,
                        ProductPageTextTitle = product.ProductPageTextTitle,
                        Product = new Product
                                        {
                                            ProductName = $"Product_{++lastIdProduct}",
                                            ProductCustomValue = $"Product_Promocion_{lastIdProductPage}",
                                            ProductImage = product.Product.ProductImage,
                                            ProductOrder = j + 1,
                                            ProductURL = "",
                                            ProductSubtitle = product.Product.ProductSubtitle
                                        },
                    });
                }

                FosePagePreview.Promocions.Add(newPromo);
            }           

            return FosePagePreview;
        }

        private async Task<PromocionPage> GetPromocionPagePreview(PromocionPagePreview preview)
        {
            PromocionPage promocion = await db.PromocionPages.FindAsync(preview.PromocionPageId);
            if (promocion == null)
            {
                var lastIdPromocions = await db.PromocionPages.MaxAsync(x => x.PromocionPageId) + 1;
                var lastProductOrder = await db.PromocionPages.Where(x => x.FosePageId == preview.FosePageId).ToListAsync();
                int maxOrder = 1;
                if (lastProductOrder.Count > 0)
                {
                    maxOrder = lastProductOrder.Max(x => x.PromocionPageOrder) + 1;
                }

                promocion = new PromocionPage
                {
                    PromocionPageActive = "EnEdicion",
                    PromocionPageCreatedDate = DateTime.Now,
                    PromocionPageCustomValue = $"Promocion_{++lastIdPromocions}",
                    PromocionPageHeadImage = preview.HeadImagePromocionPage,
                    PromocionPageTextFose = preview.TextFosePromocionPage,
                    PromocionPageTextColorFose = preview.TextColorFosePromocionPage,
                    PromocionPageOrder = maxOrder,
                    PromocionPageImageLogo1 = preview.ImageLogo1PromocionPage,
                    PromocionPageImageLogo2 = preview.ImageLogo2PromocionPage,
                    PromocionPageHeadText = preview.HeadTextPromocionPage,
                    PromocionPageSpanHeadText = preview.SpanHeadTextPromocionPage,
                    PromocionPageColorHeadBg = preview.ColorHeadBgPromocionPage,
                    PromocionPageSpanHeadtextColor = preview.SpanHeadtextColorPromocionPage,
                    PromocionPageHeadtextColor = preview.HeadtextColorPromocionPage,
                    PromocionPageSubText1 = preview.SubText1PromocionPage,
                    PromocionPageSubText2 = preview.SubText2PromocionPage,
                    FosePageId = preview.FosePageId.Value,
                    ProductPages = new List<ProductPage>()
                };
            }
            else
            {
                promocion.PromocionPageActive = "EnEdicion";
                promocion.PromocionPageImageLogo1 = preview.ImageLogo1PromocionPage;
                promocion.PromocionPageImageLogo2 = preview.ImageLogo2PromocionPage;
                promocion.PromocionPageHeadImage = preview.HeadImagePromocionPage;
                promocion.PromocionPageTextFose = preview.TextFosePromocionPage;
                promocion.PromocionPageTextColorFose = preview.TextColorFosePromocionPage;
                promocion.PromocionPageHeadText = preview.HeadTextPromocionPage;
                promocion.PromocionPageSpanHeadText = preview.SpanHeadTextPromocionPage;
                promocion.PromocionPageColorHeadBg = preview.ColorHeadBgPromocionPage;
                promocion.PromocionPageSpanHeadtextColor = preview.SpanHeadtextColorPromocionPage;
                promocion.PromocionPageHeadtextColor = preview.HeadtextColorPromocionPage;
                promocion.PromocionPageSubText1 = preview.SubText1PromocionPage;
                promocion.PromocionPageSubText2 = preview.SubText2PromocionPage;
            }

            return promocion;
        }

        private async Task<ProductPage> GetProductPagePreview(ProductPagePreview preview)
        {
            var ProductPage = await db.ProductPages.FindAsync(preview.ProductPageId);
            if (ProductPage != null)
            {
                ProductPage.Product.ProductImage = preview.ImageProductPage;
                ProductPage.Product.ProductSubtitle = preview.TextTitleProductPage;
            }
            else
            {
                var lastIdProductPage = await db.ProductPages.MaxAsync(x => x.ProductPageId) + 1;
                var lastIdProduct = await db.Products.MaxAsync(x => x.ProductId) + 1;
                var lastProductOrder = await db.ProductPages.Where(x => x.PromocionPageId == preview.PromocionPageId).ToListAsync();
                int maxOrder = 1;
                if (lastProductOrder.Count > 0)
                {
                    maxOrder = lastProductOrder.Max(x => x.Product.ProductOrder) + 1;
                }

                ProductPage = new ProductPage
                {
                    ProductPageActive = "EnEdicion",
                    ProductPageCreatedDate = DateTime.Now,
                    ProductPageCustomValue = $"ProductPage_{lastIdProductPage}",
                    PromocionPageId = preview.PromocionPageId.Value,
                    Product = new Product
                    {
                        ProductName = $"Product_{lastIdProduct}",
                        ProductCustomValue = $"Product_Promocion_{lastIdProductPage}",
                        ProductImage = preview.ImageProductPage,
                        ProductOrder = maxOrder,
                        ProductURL = "",
                        ProductSubtitle = preview.TextTitleProductPage,
                    }
                };
            }
            ProductPage.ProductPageActive = "EnEdicion";
            ProductPage.ProductPageBgColor = preview.BgColorProductPage;
            ProductPage.ProductPageTextTitle = preview.TextTitleProductPage;
            ProductPage.ProductPageColorTextTitle = preview.ColorTextTitleProductPage;
            ProductPage.ProductPageTextDescription1 = preview.TextDescription1ProductPage;
            ProductPage.ProductPageTextDescription2 = preview.TextDescription2ProductPage;
            ProductPage.ProductPageTextCharacteristic1 = preview.TextCharacteristic1ProductPage;
            ProductPage.ProductPageTextCharacteristic2 = preview.TextCharacteristic2ProductPage;
            ProductPage.ProductPageColorTextDescription1 = preview.ColorTextDescription1ProductPage;
            ProductPage.ProductPageColorTextDescription2 = preview.ColorTextDescription2ProductPage;
            ProductPage.ProductPageColorTextCharacteristic1 = preview.ColorTextCharacteristic1ProductPage;
            ProductPage.ProductPageColorTextCharacteristic2 = preview.ColorTextCharacteristic2ProductPage;

            return ProductPage;
        }

        private async Task<FosePage> GetActiveFosePage()
        {
            var h = await db.FosePages.FirstOrDefaultAsync(p => p.FosePageActive == "Activada");
            if (h != null)
            {
                h.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == h.FosePageId && x.ImageSectionPageName == "FosePage").ToListAsync();
                h.Promocions = h.Promocions.OrderBy(x => x.PromocionPageOrder).ToList();
                //foreach (var y in h.Promocions)
                //{
                //    y.HeadImage =
                //        await db.ImageSections.FirstOrDefaultAsync(x => x.ImageSectionPageId == y.PromocionPageId && x.ImageSectionPageName == "FosePromocion");
                //}

                return h;
            }
            return null;
        }

        private static void FillDataTextFosePage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as FosePagePreview;
            if (preview != null)
            {
                var formValue = httpRequest.Unvalidated.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "TextoSucursalesFose":
                        preview.TextoSucursalesFose = formValue;
                        break;
                }
            }
        }

        private static void FillDataImagePromocionPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as PromocionPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImageLogo1PromocionPage":
                        preview.ImageLogo1PromocionPage = fileName;
                        break;
                    case "ImageLogo2PromocionPage":
                        preview.ImageLogo2PromocionPage = fileName;
                        break;
                    case "HeadImagePromocionPage":
                        preview.HeadImagePromocionPage = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextPromocionPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as PromocionPagePreview;
            if (preview != null)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "HeadTextPromocionPage":
                        preview.HeadTextPromocionPage = formValue;
                        break;
                    case "SpanHeadTextPromocionPage":
                        preview.SpanHeadTextPromocionPage = formValue;
                        break;
                    case "ColorHeadBgPromocionPage":
                        preview.ColorHeadBgPromocionPage = formValue;
                        break;
                    case "SpanHeadtextColorPromocionPage":
                        preview.SpanHeadtextColorPromocionPage = formValue;
                        break;
                    case "HeadtextColorPromocionPage":
                        preview.HeadtextColorPromocionPage = formValue;
                        break;
                    case "SubText1PromocionPage":
                        preview.SubText1PromocionPage = formValue;
                        break;
                    case "SubText2PromocionPage":
                        preview.SubText2PromocionPage = formValue;
                        break;
                    case "TextFosePromocionPage":
                        preview.TextFosePromocionPage = formValue;
                        break;
                    case "TextColorFosePromocionPage":
                        preview.TextColorFosePromocionPage = formValue;
                        break;
                    case "ImageLogo1PromocionPage":
                        preview.ImageLogo1PromocionPage = "~/" + formValue.Substring(formValue.IndexOf("Content"));
                        break;
                    case "ImageLogo2PromocionPage":
                        preview.ImageLogo2PromocionPage = "~/" + formValue.Substring(formValue.IndexOf("Content"));
                        break;
                    case "HeadImagePromocionPage":
                        preview.HeadImagePromocionPage = "~/" + formValue.Substring(formValue.IndexOf("Content"));
                        break;
                }
            }
        }

        private static void FillDataImageProductPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as ProductPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImageProductPage":
                        preview.ImageProductPage = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextProductPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as ProductPagePreview;
            if (preview != null)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "BgColorProductPage":
                        preview.BgColorProductPage = formValue;
                        break;
                    case "TextTitleProductPage":
                        preview.TextTitleProductPage = formValue;
                        break;
                    case "ColorTextTitleProductPage":
                        preview.ColorTextTitleProductPage = formValue;
                        break;
                    case "TextDescription1ProductPage":
                        preview.TextDescription1ProductPage = formValue;
                        break;
                    case "TextDescription2ProductPage":
                        preview.TextDescription2ProductPage = formValue;
                        break;
                    case "TextCharacteristic1ProductPage":
                        preview.TextCharacteristic1ProductPage = formValue;
                        break;
                    case "TextCharacteristic2ProductPage":
                        preview.TextCharacteristic2ProductPage = formValue;
                        break;
                    case "ColorTextDescription1ProductPage":
                        preview.ColorTextDescription1ProductPage = formValue;
                        break;
                    case "ColorTextDescription2ProductPage":
                        preview.ColorTextDescription2ProductPage = formValue;
                        break;
                    case "ColorTextCharacteristic1ProductPage":
                        preview.ColorTextCharacteristic1ProductPage = formValue;
                        break;
                    case "ColorTextCharacteristic2ProductPage":
                        preview.ColorTextCharacteristic2ProductPage = formValue;
                        break;
                    case "ImageProductPage":
                        preview.ImageProductPage = "~/" + formValue.Substring(formValue.IndexOf("Content"));
                        break;
                }
            }
        }

    }
}