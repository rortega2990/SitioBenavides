using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Linq;
using System;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using System.Web;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomePageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var homePages = db.HomePages.Include(h => h.BlogSection)
                .Include(h => h.CardSection).Include(h => h.FoseSection).Include(h => h.OfferSection)
                .Include(h => h.FourQuadSection).Include(h => h.DoctorsOfficeSection);
            var homePageModel = await homePages.ToListAsync();
            return View(homePageModel);
        }

        [HttpGet]
        public async Task<ActionResult> SetState(int? id)
        {
            var HomePage = await db.HomePages.FindAsync(id);

            if (HomePage != null)
            {
                var HomePageActive = await GetActiveHomePage();
                if (HomePageActive != null)
                {
                    if (!HomePage.HomePageActive)
                    {
                        HomePage.HomePageActive = true;
                        HomePageActive.HomePageActive = false;
                        db.Entry(HomePage).State = EntityState.Modified;
                        db.Entry(HomePageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        var HomePages = db.HomePages.Include(h => h.BlogSection).Include(h => h.CardSection).Include(h => h.FoseSection).Include(h => h.OfferSection);
                        return View("Index", await HomePages.ToListAsync());
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Preview(int? id)
        {
            HomePage homePage;
            if(id == null)
            {
                homePage = await GetActiveHomePage();
                if (homePage != null)
                    return View(homePage);
            }
            homePage = await db.HomePages.FindAsync(id);
            if (homePage != null)
            {
                homePage.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == homePage.HomePageId &&  x.ImageSectionPageName == "HomePage").ToListAsync();
                homePage.BlogSection.Products = homePage.BlogSection.Products.OrderBy(x => x.ProductOrder).ToList();
                homePage.OfferSection.Products = homePage.OfferSection.Products.OrderBy(x => x.ProductOrder).ToList();
                foreach (var p in homePage.OfferSection.Products)
                {
                    var offerId = await db.OfferPages.FirstAsync(x => x.OfferPageActive && x.OfferTypeId == p.OfferTypeId);
                    if (offerId != null)
                    {
                        p.ProductURL = "~/Offer/OfferType?id=" + offerId.OfferPageId;
                    }
                }

                foreach (var p in homePage.BlogSection.Products)
                {
                    var blogId = await db.BlogPages.FirstAsync(x => x.BlogPageActive == "Activada" && x.BlogTypeId == p.BlogTypeId);
                    if (blogId != null)
                    {
                        p.ProductURL = "~/Blog/BlogType?id=" + blogId.BlogTypeId;
                    }
                }

                return View(homePage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit()
        {
            var previewHome = await db.PagePreviews.FindAsync("PreviewHome");

            var str = new MemoryStream(previewHome.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as HomePagePreview;

            var homePageActive = await GetActiveHomePage();
            var lastIdHomePage = await db.HomePages.MaxAsync(x => x.HomePageId) + 1;
            var lastIdProduct = await db.Products.MaxAsync(x => x.ProductId);
            var lastIdTitle = await db.TitleTypes.MaxAsync(x => x.TitleTypeId);
            var lastIdOffer = await db.OfferSections.MaxAsync(x => x.OfferSectionId);
            var lastIdCard = await db.CardSections.MaxAsync(x => x.CardSectionId);
            var lastIdMakeup = await db.FoseSections.MaxAsync(x => x.FoseSectionId);
            var lastIdBlog = await db.BlogSections.MaxAsync(x => x.BlogSectionId);

            if (preview == null)
            {
                if (homePageActive != null)
                    return View("Preview", homePageActive);
            }
            else
            {
                var homePagePreview = GetHomePagePreview(homePageActive, preview, lastIdHomePage, lastIdProduct, lastIdTitle, lastIdOffer, lastIdCard, lastIdMakeup, lastIdBlog);
                return View("Preview", homePagePreview);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("HomePage", "~/UploadedFiles", "PreviewHome", new HomePagePreview { HomePageName = "PreviewHome" }, FileType.Image, FillDataTextHomePage, FillDataImageHomePage);

            if(result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var homePageActive = await GetActiveHomePage();
            return View(homePageActive);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("HomePage", "~/Content/rsc/imgs", "PreviewHome", new HomePagePreview { HomePageName = "PreviewHome" }, FileType.Image, FillDataTextHomePage, FillDataImageHomePage);

            if (result.Key)
            {
                PagePreview previewHome = await db.PagePreviews.FindAsync("PreviewHome");

                var str = new MemoryStream(previewHome.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as HomePagePreview;

                if (preview != null)
                {
                    var homePageActive = await GetActiveHomePage();
                    var lastIdHomePage = await db.HomePages.MaxAsync(x => x.HomePageId) + 1;
                    var lastIdProduct = await db.Products.MaxAsync(x => x.ProductId);
                    var lastIdTitle = await db.TitleTypes.MaxAsync(x => x.TitleTypeId);
                    var lastIdOffer = await db.OfferSections.MaxAsync(x => x.OfferSectionId);
                    var lastIdCard = await db.CardSections.MaxAsync(x => x.CardSectionId);
                    var lastIdMakeup = await db.FoseSections.MaxAsync(x => x.FoseSectionId);
                    var lastIdBlog = await db.BlogSections.MaxAsync(x => x.BlogSectionId);
                    var homePageEdit = GetHomePagePreview(homePageActive, preview, lastIdHomePage, lastIdProduct, lastIdTitle, lastIdOffer, lastIdCard, lastIdMakeup, lastIdBlog);
                    var id = db.HomePages.Add(homePageEdit);
                    await db.SaveChangesAsync();
                    foreach (var h in homePageEdit.HeadImages)
                    {
                        h.ImageSectionPageId = id.HomePageId;
                        db.ImageSections.Add(h);
                    }
                    await db.SaveChangesAsync();
                    homePageActive.HomePageActive = false;
                    db.Entry(homePageActive).State = EntityState.Modified;
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

            var page = db.HomePages.Where(p => p.HomePageId == id).FirstOrDefault();

            if(page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.HomePageCreatedDate,
                Id = page.HomePageId,
                Active = page.HomePageActive,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.HomePageCustomValue,
                Title = "Eliminar página de inicio"
            };
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            var page = db.HomePages.Where(p => p.HomePageId == id).FirstOrDefault();
            if(page != null && page.HomePageActive == false)
            {
                db.HomePages.Remove(page);
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

        private HomePage GetHomePagePreview(HomePage homePageActive, HomePagePreview preview, int lastIdHomePage, int lastIdProduct, int lastIdTitle, int lastIdOffer, int lastIdCard, int lastIdMakeup, int lastIdBlog)
        {
            var homePagePreview = new HomePage
            {
                FourQuadSection = new FourQuadSection() {
                    Quad1 = new SingleQuadSectionItem() {
                        Text = preview.QuadText1,
                        BackgroundColor = preview.QuadBackgroundColor1,
                        Link = preview.QuadLink1,
                        ImageFileName = preview.QuadImageFileName1 ?? homePageActive.FourQuadSection.Quad1.ImageFileName
                    },
                    Quad2 = new SingleQuadSectionItem()
                    {
                        Text = preview.QuadText2,
                        BackgroundColor = preview.QuadBackgroundColor2,
                        Link = preview.QuadLink2,
                        ImageFileName = preview.QuadImageFileName2 ?? homePageActive.FourQuadSection.Quad2.ImageFileName
                    },
                    Quad3 = new SingleQuadSectionItem()
                    {
                        Text = preview.QuadText3,
                        BackgroundColor = preview.QuadBackgroundColor3,
                        Link = preview.QuadLink3,
                        ImageFileName = preview.QuadImageFileName3 ?? homePageActive.FourQuadSection.Quad3.ImageFileName
                    },
                    Quad4 = new SingleQuadSectionItem()
                    {
                        Text = preview.QuadText4,
                        BackgroundColor = preview.QuadBackgroundColor4,
                        Link = preview.QuadLink4,
                        ImageFileName = preview.QuadImageFileName4 ?? homePageActive.FourQuadSection.Quad4.ImageFileName
                    },
                },
                DoctorsOfficeSection = new DoctorsOfficeSection() {
                    Link = preview.DoctorsOfficeLink ?? homePageActive.DoctorsOfficeSection.Link,
                    BackgroundColor = preview.DoctorsOfficeBackgroundColor ?? homePageActive.DoctorsOfficeSection.BackgroundColor,
                    ImageFileName = preview.DoctorsOfficeImageFileName ?? homePageActive.DoctorsOfficeSection.ImageFileName,
                    LogoImageFileName = preview.DoctorsOfficeLogoImageFileName ?? homePageActive.DoctorsOfficeSection.LogoImageFileName,
                    SectionMessageText = preview.DoctorsOfficeMessageText ?? homePageActive.DoctorsOfficeSection.SectionMessageText,
                    SectionMessageTextColor = preview.DoctorsOfficeMessageTextColor ?? homePageActive.DoctorsOfficeSection.SectionMessageTextColor,
                    Title = preview.DoctorsOfficeTitle ?? homePageActive.DoctorsOfficeSection.Title,
                    TitleColor = preview.DoctorsOfficeTitleColor ?? homePageActive.DoctorsOfficeSection.TitleColor
                },
                HomePageCustomValue = $"HomePage_{lastIdHomePage}",
                HomePageTitle = "Inicio",
                HomePageActive = true,
                HomePageCreatedDate = DateTime.Now,
                HeadImages = Utils.ConvertToImageSectionList(preview.Encabezado),
                OfferSection = new OfferSection
                {
                    OfferSectionCustomValue = $"OfferSection_{++lastIdOffer}",
                    OfferSectionTitle = preview.TituloOfertas ?? homePageActive.OfferSection.OfferSectionTitle,
                    OfferSectionColorTitle = preview.ColorTituloOfertas ?? homePageActive.OfferSection.OfferSectionColorTitle,
                    Products = new List<Product>
                        {
                            new Product
                            {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_OfferSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType {
                                    TitleTypeColor = preview.ColorImagenOfertas1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeColor,
                                    TitleTypeMessage = preview.TextoImagenOfertas1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}",
                                },
                                ProductImage = preview.ImagenOfertas1 ?? homePageActive.OfferSection.Products[0].ProductImage,
                                OfferTypeId = string.IsNullOrEmpty(preview.TipoOfertas1) ? homePageActive.OfferSection.Products[0].OfferTypeId : int.Parse(preview.TipoOfertas1),
                                ProductOrder = 1,
                            },
                            new Product
                            {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_OfferSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType {
                                    TitleTypeColor = preview.ColorImagenOfertas2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeColor,
                                    TitleTypeMessage = preview.TextoImagenOfertas2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}"
                                },
                                ProductImage = preview.ImagenOfertas2 ?? homePageActive.OfferSection.Products[1].ProductImage,
                                OfferTypeId = string.IsNullOrEmpty(preview.TipoOfertas2) ? homePageActive.OfferSection.Products[1].OfferTypeId : int.Parse(preview.TipoOfertas2),
                                ProductOrder = 2,
                             },
                             new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_OfferSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType {
                                    TitleTypeColor = preview.ColorImagenOfertas3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeColor,
                                    TitleTypeMessage = preview.TextoImagenOfertas3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}"
                                },
                                ProductImage = preview.ImagenOfertas3 ?? homePageActive.OfferSection.Products[2].ProductImage,
                                OfferTypeId = string.IsNullOrEmpty(preview.TipoOfertas3) ? homePageActive.OfferSection.Products[2].OfferTypeId : int.Parse(preview.TipoOfertas3),
                                ProductOrder = 3,
                             },
                             new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_OfferSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType {
                                    TitleTypeColor = preview.ColorImagenOfertas4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeColor,
                                    TitleTypeMessage = preview.TextoImagenOfertas4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}"
                                },
                                ProductImage = preview.ImagenOfertas4 ?? homePageActive.OfferSection.Products[3].ProductImage,
                                OfferTypeId = string.IsNullOrEmpty(preview.TipoOfertas4) ? homePageActive.OfferSection.Products[3].OfferTypeId : int.Parse(preview.TipoOfertas4),
                                ProductOrder = 4,
                             },
                             new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_OfferSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType {
                                    TitleTypeColor = preview.ColorImagenOfertas5 ?? homePageActive.OfferSection.Products[4].ProductTitle.TitleTypeColor,
                                    TitleTypeMessage = preview.TextoImagenOfertas5 ?? homePageActive.OfferSection.Products[4].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas5 ?? homePageActive.OfferSection.Products[4].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}"
                                },
                                ProductImage = preview.ImagenOfertas5 ?? homePageActive.OfferSection.Products[4].ProductImage,
                                OfferTypeId = string.IsNullOrEmpty(preview.TipoOfertas5) ? homePageActive.OfferSection.Products[4].OfferTypeId : int.Parse(preview.TipoOfertas5),
                                ProductOrder = 5,
                             },
                        }
                },
                CardSection = new CardSection
                {
                    CardSectionCustomValue = $"CardSection_{++lastIdCard}",
                    CardSectionTitle = preview.TituloTarjetas ?? homePageActive.CardSection.CardSectionTitle,
                    CardSectionColorTitle = preview.ColorTituloTarjetas ?? homePageActive.CardSection.CardSectionColorTitle,
                    CardSectionUrl = preview.UrlImagenTarjetas ?? homePageActive.CardSection.CardSectionUrl,
                    CardSectionImage = preview.ImagenTarjetasWeb ?? homePageActive.CardSection.CardSectionImage,
                    CardSectionImageXS = preview.ImagenTarjetasMovil ?? homePageActive.CardSection.CardSectionImageXS
                },
                FoseSection = new FoseSection
                {
                    FoseSectionCustomValue = $"MakeupSection_{++lastIdMakeup}",
                    FoseSectionTitle = preview.TituloFose ?? homePageActive.FoseSection.FoseSectionTitle,
                    FoseSectionColorTitle = preview.ColorTituloFose ?? homePageActive.FoseSection.FoseSectionColorTitle,
                    FoseSectionWord1 = preview.TextoFose1 ?? homePageActive.FoseSection.FoseSectionWord1,
                    FoseSectionColorWord1 = preview.ColorTextoFose1 ?? homePageActive.FoseSection.FoseSectionColorWord1,
                    FoseSectionWord2 = preview.TextoFose2 ?? homePageActive.FoseSection.FoseSectionWord2,
                    FoseSectionColorWord2 = preview.ColorTextoFose2 ?? homePageActive.FoseSection.FoseSectionColorWord2,
                    FoseSectionImageLogo = preview.ImagenFoseLogo ?? homePageActive.FoseSection.FoseSectionImageLogo,
                    FoseSectionImage = preview.ImagenFoseProducto ?? homePageActive.FoseSection.FoseSectionImage
                },
                BlogSection = new BlogSection
                {
                    BlogSectionCustomValue = $"BlogSection_{++lastIdBlog}",
                    BlogSectionTitle = preview.TituloBlog ?? homePageActive.BlogSection.BlogSectionTitle,
                    BlogSectionColorTitle = preview.ColorTituloBlog ?? homePageActive.BlogSection.BlogSectionColorTitle,                    
                    Products = new List<Product>
                        {
                            new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_BlogSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType
                                {
                                    TitleTypeMessage = preview.TextoBlog1 ?? homePageActive.BlogSection.Products[0].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas1 ?? homePageActive.BlogSection.Products[0].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}",
                                    TitleTypeColor = preview.ColorTextoBlog1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeColor,
                                    TitleTypeBgColor = preview.ColorBgTextoBlog1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeBgColor,
                                    TitleTypeHoverColor= preview.ColorOverTextoBlog1 ?? homePageActive.OfferSection.Products[0].ProductTitle.TitleTypeBgColor,                                    
                                },
                                ProductImage = preview.ImagenBlog1 ?? homePageActive.BlogSection.Products[0].ProductImage,
                                ProductOrder = 1,
                                ProductURL = "",
                                BlogTypeId = string.IsNullOrEmpty(preview.TipoBlog1) ? homePageActive.OfferSection.Products[0].BlogTypeId : int.Parse(preview.TipoBlog1),
                             },
                             new Product
                             {
                                 ProductName = $"Product_{++lastIdProduct}",
                                 ProductCustomValue = $"Product_BlogSection_HomPage_{lastIdHomePage}",
                                 ProductTitle = new TitleType
                                 {
                                    TitleTypeMessage = preview.TextoBlog2 ?? homePageActive.BlogSection.Products[1].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas2 ?? homePageActive.BlogSection.Products[1].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}",
                                    TitleTypeColor = preview.ColorTextoBlog2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeColor,
                                    TitleTypeBgColor = preview.ColorBgTextoBlog2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeBgColor,
                                    TitleTypeHoverColor= preview.ColorOverTextoBlog2 ?? homePageActive.OfferSection.Products[1].ProductTitle.TitleTypeBgColor,
                                 },
                                 ProductImage = preview.ImagenBlog2 ?? homePageActive.BlogSection.Products[1].ProductImage,
                                 ProductOrder = 2,
                                 ProductURL = "",
                                 BlogTypeId = string.IsNullOrEmpty(preview.TipoBlog2) ? homePageActive.OfferSection.Products[1].BlogTypeId : int.Parse(preview.TipoBlog2),
                             },
                             new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_BlogSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType
                                 {
                                    TitleTypeMessage = preview.TextoBlog3 ?? homePageActive.BlogSection.Products[2].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas3 ?? homePageActive.BlogSection.Products[2].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}",
                                    TitleTypeColor = preview.ColorTextoBlog3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeColor,
                                    TitleTypeBgColor = preview.ColorBgTextoBlog3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeBgColor,
                                    TitleTypeHoverColor= preview.ColorOverTextoBlog3 ?? homePageActive.OfferSection.Products[2].ProductTitle.TitleTypeHoverColor,
                                 },
                                ProductImage = preview.ImagenBlog3 ?? homePageActive.BlogSection.Products[2].ProductImage,
                                ProductOrder = 3,
                                ProductURL = "",
                                BlogTypeId = string.IsNullOrEmpty(preview.TipoBlog3) ? homePageActive.OfferSection.Products[2].BlogTypeId : int.Parse(preview.TipoBlog3),
                             },
                             new Product
                             {
                                ProductName = $"Product_{++lastIdProduct}",
                                ProductCustomValue = $"Product_BlogSection_HomPage_{lastIdHomePage}",
                                ProductTitle = new TitleType
                                 {
                                    TitleTypeMessage = preview.TextoBlog4 ?? homePageActive.BlogSection.Products[3].ProductTitle.TitleTypeMessage,
                                    TitleTypeSpan = preview.SpanImagenOfertas4 ?? homePageActive.BlogSection.Products[3].ProductTitle.TitleTypeSpan,
                                    TitleTypeCustomValue = $"TitleType_{++lastIdTitle}",
                                    TitleTypeColor = preview.ColorTextoBlog4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeColor,
                                    TitleTypeBgColor = preview.ColorBgTextoBlog4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeBgColor,
                                    TitleTypeHoverColor= preview.ColorOverTextoBlog4 ?? homePageActive.OfferSection.Products[3].ProductTitle.TitleTypeHoverColor,
                                 },
                                ProductImage = preview.ImagenBlog4 ?? homePageActive.BlogSection.Products[3].ProductImage,
                                ProductOrder = 4,
                                ProductURL = "",
                                BlogTypeId = string.IsNullOrEmpty(preview.TipoBlog4) ? homePageActive.OfferSection.Products[3].BlogTypeId : int.Parse(preview.TipoBlog4),
                             }
                        }
                },
            };

            return homePagePreview;
        }

        private async Task<HomePage> GetActiveHomePage()
        {
            var h = await db.HomePages.Include(hp => hp.FourQuadSection).Include(hp => hp.DoctorsOfficeSection).FirstOrDefaultAsync(p => p.HomePageActive);
            if (h != null)
            {
                h.HeadImages = await db.ImageSections.Where(x => x.ImageSectionPageId == h.HomePageId && x.ImageSectionPageName == "HomePage").ToListAsync();

                h.BlogSection.Products = h.BlogSection.Products.OrderBy(x => x.ProductOrder).ToList();
                h.OfferSection.Products = h.OfferSection.Products.OrderBy(x => x.ProductOrder).ToList();
                h.OfferTypes = await db.OfferTypes.ToListAsync();
                h.BlogTypes = await db.BlogTypes.ToListAsync();
                foreach (var p in h.OfferSection.Products)
                {
                    var offerId = await db.OfferPages.FirstAsync(x => x.OfferPageActive && x.OfferTypeId == p.OfferTypeId);
                    if (offerId != null)
                    {
                        p.ProductURL = "~/Offer/OfferType?id=" + offerId.OfferPageId;
                    }
                }

                foreach (var p in h.BlogSection.Products)
                {
                    var blogId = await db.BlogPages.FirstAsync(x => x.BlogPageActive == "Activada" && x.BlogTypeId == p.BlogTypeId);
                    if (blogId != null)
                    {
                        p.ProductURL = "~/Blog/BlogType?id=" + blogId.BlogPageId;
                    }
                }

                return h;
            }
            return null;
        }

        private static void FillDataImageHomePage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as HomePagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImagenOfertas1":
                        preview.ImagenOfertas1 = fileName;
                        break;
                    case "ImagenOfertas2":
                        preview.ImagenOfertas2 = fileName;
                        break;
                    case "ImagenOfertas3":
                        preview.ImagenOfertas3 = fileName;
                        break;
                    case "ImagenOfertas4":
                        preview.ImagenOfertas4 = fileName;
                        break;
                    case "ImagenOfertas5":
                        preview.ImagenOfertas5 = fileName;
                        break;
                    case "ImagenTarjetasWeb":
                        preview.ImagenTarjetasWeb = fileName;
                        break;
                    case "ImagenTarjetasMovil":
                        preview.ImagenTarjetasMovil = fileName;
                        break;
                    case "ImagenFoseLogo":
                        preview.ImagenFoseLogo = fileName;
                        break;
                    case "ImagenFoseProducto":
                        preview.ImagenFoseProducto = fileName;
                        break;
                    case "ImagenBlog1":
                        preview.ImagenBlog1 = fileName;
                        break;
                    case "ImagenBlog2":
                        preview.ImagenBlog2 = fileName;
                        break;
                    case "ImagenBlog3":
                        preview.ImagenBlog3 = fileName;
                        break;
                    case "ImagenBlog4":
                        preview.ImagenBlog4 = fileName;
                        break;
                    case "QuadImageFileName1":
                        preview.QuadImageFileName1 = fileName;
                        break;
                    case "QuadImageFileName2":
                        preview.QuadImageFileName2 = fileName;
                        break;
                    case "QuadImageFileName3":
                        preview.QuadImageFileName3 = fileName;
                        break;
                    case "QuadImageFileName4":
                        preview.QuadImageFileName4 = fileName;
                        break;
                    case "DoctorsOfficeImageFileName":
                        preview.DoctorsOfficeImageFileName = fileName;
                        break;
                    case "DoctorsOfficeLogoImageFileName":
                        preview.DoctorsOfficeLogoImageFileName = fileName;
                        break;
       
                 }
            }
        }

        private static void FillDataTextHomePage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as HomePagePreview;
            if (preview != null)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "TituloOfertas":
                        preview.TituloOfertas = formValue;
                        break;
                    case "ColorTituloOfertas":
                        preview.ColorTituloOfertas = formValue;
                        break;
                    case "TextoImagenOfertas1":
                        preview.TextoImagenOfertas1 = formValue;
                        break;
                    case "SpanImagenOfertas1":
                        preview.SpanImagenOfertas1 = formValue;
                        break;
                    case "ColorImagenOfertas1":
                        preview.ColorImagenOfertas1 = formValue;
                        break;
                    case "TipoOfertas1":
                        preview.TipoOfertas1 = formValue;
                        break;
                    case "TextoImagenOfertas2":
                        preview.TextoImagenOfertas2 = formValue;
                        break;
                    case "SpanImagenOfertas2":
                        preview.SpanImagenOfertas2 = formValue;
                        break;
                    case "ColorImagenOfertas2":
                        preview.ColorImagenOfertas2 = formValue;
                        break;
                    case "TipoOfertas2":
                        preview.TipoOfertas2 = formValue;
                        break;
                    case "TextoImagenOfertas3":
                        preview.TextoImagenOfertas3 = formValue;
                        break;
                    case "SpanImagenOfertas3":
                        preview.SpanImagenOfertas3 = formValue;
                        break;
                    case "ColorImagenOfertas3":
                        preview.ColorImagenOfertas3 = formValue;
                        break;
                    case "TipoOfertas3":
                        preview.TipoOfertas3 = formValue;
                        break;
                    case "TextoImagenOfertas4":
                        preview.TextoImagenOfertas4 = formValue;
                        break;
                    case "SpanImagenOfertas4":
                        preview.SpanImagenOfertas4 = formValue;
                        break;
                    case "ColorImagenOfertas4":
                        preview.ColorImagenOfertas4 = formValue;
                        break;
                    case "TipoOfertas4":
                        preview.TipoOfertas4 = formValue;
                        break;
                    case "TextoImagenOfertas5":
                        preview.TextoImagenOfertas5 = formValue;
                        break;
                    case "SpanImagenOfertas5":
                        preview.SpanImagenOfertas5 = formValue;
                        break;
                    case "ColorImagenOfertas5":
                        preview.ColorImagenOfertas5 = formValue;
                        break;
                    case "TipoOfertas5":
                        preview.TipoOfertas5 = formValue;
                        break;
                    case "TituloTarjetas":
                        preview.TituloTarjetas = formValue;
                        break;
                    case "ColorTituloTarjetas":
                        preview.ColorTituloTarjetas = formValue;
                        break;
                    case "UrlImagenTarjetas":
                        preview.UrlImagenTarjetas = formValue;
                        break;
                    case "TituloFose":
                        preview.TituloFose = formValue;
                        break;
                    case "ColorTituloFose":
                        preview.ColorTituloFose = formValue;
                        break;
                    case "TextoFose1":
                        preview.TextoFose1 = formValue;
                        break;
                    case "ColorTextoFose1":
                        preview.ColorTextoFose1 = formValue;
                        break;
                    case "TextoFose2":
                        preview.TextoFose2 = formValue;
                        break;
                    case "ColorTextoFose2":
                        preview.ColorTextoFose2 = formValue;
                        break;
                    case "TituloBlog":
                        preview.TituloBlog = formValue;
                        break;
                    case "ColorTituloBlog":
                        preview.ColorTituloBlog = formValue;
                        break;
                    case "TextoBlog1":
                        preview.TextoBlog1 = formValue;
                        break;
                    case "UrlImagenBlog1":
                        preview.UrlImagenBlog1 = formValue;
                        break;
                    case "ColorTextoBlog1":
                        preview.ColorTextoBlog1 = formValue;
                        break;
                    case "ColorBgTextoBlog1":
                        preview.ColorBgTextoBlog1 = formValue;
                        break;
                    case "ColorOverTextoBlog1":
                        preview.ColorOverTextoBlog1 = formValue;
                        break;
                    case "TextoBlog2":
                        preview.TextoBlog2 = formValue;
                        break;
                    case "UrlImagenBlog2":
                        preview.UrlImagenBlog2 = formValue;
                        break;
                    case "ColorTextoBlog2":
                        preview.ColorTextoBlog2 = formValue;
                        break;
                    case "ColorBgTextoBlog2":
                        preview.ColorBgTextoBlog2 = formValue;
                        break;
                    case "ColorOverTextoBlog2":
                        preview.ColorOverTextoBlog2 = formValue;
                        break;
                    case "TextoBlog3":
                        preview.TextoBlog3 = formValue;
                        break;
                    case "UrlImagenBlog3":
                        preview.UrlImagenBlog3 = formValue;
                        break;
                    case "ColorTextoBlog3":
                        preview.ColorTextoBlog3 = formValue;
                        break;
                    case "ColorBgTextoBlog3":
                        preview.ColorBgTextoBlog3 = formValue;
                        break;
                    case "ColorOverTextoBlog3":
                        preview.ColorOverTextoBlog3 = formValue;
                        break;
                    case "TextoBlog4":
                        preview.TextoBlog4 = formValue;
                        break;
                    case "UrlImagenBlog4":
                        preview.UrlImagenBlog4 = formValue;
                        break;
                    case "ColorTextoBlog4":
                        preview.ColorTextoBlog4 = formValue;
                        break;
                    case "ColorBgTextoBlog4":
                        preview.ColorBgTextoBlog4 = formValue;
                        break;
                    case "ColorOverTextoBlog4":
                        preview.ColorOverTextoBlog4 = formValue;
                        break;

                    case "TipoBlog1":
                        preview.TipoBlog1 = formValue;
                        break;

                    case "TipoBlog2":
                        preview.TipoBlog2 = formValue;
                        break;

                    case "TipoBlog3":
                        preview.TipoBlog3 = formValue;
                        break;

                    case "TipoBlog4":
                        preview.TipoBlog4 = formValue;
                        break;
                    case "QuadBackgroundColor1":
                        preview.QuadBackgroundColor1 = formValue;
                        break;
                    case "QuadText1":
                        preview.QuadText1 = formValue;
                        break;
                    case "QuadLink1":
                        preview.QuadLink1 = formValue;
                        break;
                    

                    case "QuadBackgroundColor2":
                        preview.QuadBackgroundColor2 = formValue;
                        break;
                    case "QuadText2":
                        preview.QuadText2 = formValue;
                        break;
                    case "QuadLink2":
                        preview.QuadLink2 = formValue;
                        break;
                   

                    case "QuadBackgroundColor3":
                        preview.QuadBackgroundColor3 = formValue;
                        break;
                    case "QuadText3":
                        preview.QuadText3 = formValue;
                        break;
                    case "QuadLink3":
                        preview.QuadLink3 = formValue;
                        break;
                    

                    case "QuadBackgroundColor4":
                        preview.QuadBackgroundColor4 = formValue;
                        break;
                    case "QuadText4":
                        preview.QuadText4 = formValue;
                        break;
                    case "QuadLink4":
                        preview.QuadLink4 = formValue;
                        break;

                    case "DoctorsOfficeTitle":
                        preview.DoctorsOfficeTitle = formValue;
                        break;
                    case "DoctorsOfficeMessageText":
                        preview.DoctorsOfficeMessageText = formValue;
                        break;

                    case "DoctorsOfficeBackgroundColor":
                        preview.DoctorsOfficeBackgroundColor = formValue;
                        break;
                    case "DoctorsOfficeLink":
                        preview.DoctorsOfficeLink = formValue;
                        break;
                    case "DoctorsOfficeTitleColor":
                        preview.DoctorsOfficeTitleColor = formValue;
                        break;
                    case "DoctorsOfficeMessageTextColor":
                        preview.DoctorsOfficeMessageTextColor = formValue;
                        break;

                    
                }
            }
        }
    }
}
