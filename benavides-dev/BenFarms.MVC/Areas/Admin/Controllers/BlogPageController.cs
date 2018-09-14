using System;
using System.Collections.Generic;
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
using System.Web.Routing;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var BlogPages = db.BlogPages.Include(x=>x.BlogType);
            return View(await BlogPages.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> ListNews(int? idBlog, int? idNews, int? v)
        {
            ViewBag.Listar = v;
            ViewBag.Blog = idBlog;
            if (idBlog != null)
            {
                BlogPage blogPage = await db.BlogPages.FindAsync(idBlog);

                if (blogPage != null)
                {                    
                    ViewBag.BlogPage = blogPage.BlogPageCustomValue;
                    return View(blogPage.News);
                }
            }
            if (idNews != null)
            {
                NewsPage newsPage = await db.NewsPages.FindAsync(idNews);

                if (newsPage != null)
                {
                    BlogPage blogPage = await db.BlogPages.FindAsync(newsPage.BlogPageId);

                    if (blogPage != null)
                    {
                        ViewBag.BlogPage = blogPage.BlogPageCustomValue;
                        return View(blogPage.News);
                    }
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Active(int id)
        {
            BlogPage BlogPage = await db.BlogPages.FindAsync(id);
            if (BlogPage != null)
            {
                if (BlogPage.BlogPageActive == "EnEdicion" || BlogPage.BlogPageActive == "Desactivada")
                {
                    return View("ActiveConfirmed", BlogPage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("BillingPageActive")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveConfirmed(int id)
        {
            var BlogPage = await db.BlogPages.FindAsync(id);
            if (BlogPage != null)
            {                
                var BlogPageActive = await GetActiveBlogPage(BlogPage.BlogTypeId);
                if (BlogPageActive != null)
                {
                    if (BlogPage.BlogPageActive == "EnEdicion" || BlogPage.BlogPageActive == "Desactivada")
                    {
                        BlogPage.BlogPageActive = "Activada";
                        foreach (var newsPage in BlogPage.News)
                        {
                            newsPage.NewsPageActive = "Activada";
                        }

                        BlogPageActive.BlogPageActive = "Desactivada";
                        foreach (var newsPage in BlogPageActive.News)
                        {
                            newsPage.NewsPageActive = "Desactivada";
                        }

                        db.Entry(BlogPage).State = EntityState.Modified;
                        db.Entry(BlogPageActive).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
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
            BlogPage BlogPage = await db.BlogPages.FindAsync(id);
            if (BlogPage != null)
            {
                BlogPage.BlogTypes = await db.BlogTypes.ToListAsync();
                BlogPage.News = BlogPage.News.OrderBy(x => x.NewsPageOrder).ToList();
                ViewBag.BlogId = id;
                return View(BlogPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewBlog(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            BlogPage BlogPage = await db.BlogPages.FindAsync(id);
            if (BlogPage != null)
            {
                BlogPage.BlogTypes = await db.BlogTypes.ToListAsync();
                BlogPage.News = BlogPage.News.OrderBy(x => x.NewsPageOrder).ToList();
                ViewBag.BlogId = id;
                return View(BlogPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewNews(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var NewsPage = await db.NewsPages.FindAsync(id);
            if (NewsPage != null)
            {
                return View(NewsPage);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            BlogPage BlogPage = await db.BlogPages.FindAsync(id);
            if (BlogPage != null)
            {
                if (BlogPage.BlogPageActive == "EnEdicion")
                {
                    return View("DeleteConfirmed", BlogPage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var BlogPage = await db.BlogPages.FindAsync(id);

            if (BlogPage != null)
            {
                if (BlogPage.BlogPageActive == "EnEdicion")
                {
                    db.BlogPages.Remove(BlogPage);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteNews(int? idNews, int? idBlog)
        {
            ViewBag.Blog = idBlog;
            NewsPage NewsPage = await db.NewsPages.FindAsync(idNews);
            if (NewsPage != null)
            {
                if (NewsPage.NewsPageActive == "EnEdicion")
                {
                    return View("DeleteNewsConfirmed", NewsPage);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteNewsConfirmed(int? idNews, int? idBlog)
        {
            var NewsPage = await db.NewsPages.FindAsync(idNews);
            if (NewsPage != null)
            {
                var r = new RouteValueDictionary {{"idBlog", NewsPage.BlogPageId}};
                if (NewsPage.NewsPageActive == "EnEdicion")
                {
                    db.NewsPages.Remove(NewsPage);
                    await db.SaveChangesAsync();                    
                    return RedirectToAction("ListNews", r);
                }
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEdit(int? id)
        {
            var previewBlog = await db.PagePreviews.FindAsync("PreviewBlog");

            var str = new MemoryStream(previewBlog.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as BlogPagePreview;             
            
            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var BlogPagePreview = await GetBlogPagePreview(preview);            
            return View("Preview", BlogPagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEdit(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BlogPage", "~/UploadedFiles", "PreviewBlog", new BlogPagePreview { BlogPageName = "PreviewBlog" }, FileType.ImagePdf, FillDataTextBlogPage, FillDataImageBlogPage);

            if(result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public async Task<ActionResult> PreviewEditNews()
        {
            var previewNews = await db.PagePreviews.FindAsync("PreviewNews");

            var str = new MemoryStream(previewNews.PageValue);
            var binaryFormatter = new BinaryFormatter();
            var preview = binaryFormatter.Deserialize(str) as NewsPagePreview;

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var NewsPagePreview = await GetNewsPagePreview(preview);
            return View("PreviewNews", NewsPagePreview);
        }

        [HttpPost]
        public async Task<ActionResult> PreviewEditNews(int? idNews, int? idBlog)
        {
            var result = await Utils.PrepareDataPage("NewsPage", "~/UploadedFiles", "PreviewNews", new NewsPagePreview { NewsPageName = "PreviewNews", NewsPageId = idNews }, FileType.Image, FillDataTextNewsPage, FillDataImageNewsPage);
            if (result.Key)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            BlogPage BlogPageActive;
            if (id == null)
            {
                BlogPageActive = await GetActiveBlogPage(1);
                return View(BlogPageActive);
            }
            BlogPageActive = await GetActiveBlogPage(id.Value);
            if (BlogPageActive != null)
            {
                return View(BlogPageActive);
            }
            return RedirectToAction("NotFound", "Error");
        }

        [HttpGet]
        public async Task<ActionResult> EditNews(int? idNews, int? idBlog)
        {
            ViewBag.Blog = idBlog;
            NewsPage page = (idNews != null) ? await db.NewsPages.FindAsync(idNews) : new NewsPage();
            return View(page);
        }

        [HttpGet]
        public async Task<ActionResult> EditAjax(int id)
        {
            var response = new AjaxResponse { Success = false, Message = "No existe la categoría"};

            try
            {
                BlogPage BlogPageActive = await GetActiveBlogPage(id);
                if (BlogPageActive != null)
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

        [HttpPost]
        public async Task<ActionResult> ApplyView(string v, string p)
        {
            var result = await Utils.PrepareDataPage("BlogPage", "~/Content/rsc/imgs", "PreviewBlog", new BlogPagePreview { BlogPageName = "PreviewBlog" }, FileType.ImagePdf, FillDataTextBlogPage, FillDataImageBlogPage);

            if (result.Key)
            {
                PagePreview previewBlog = await db.PagePreviews.FindAsync("PreviewBlog");

                var str = new MemoryStream(previewBlog.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as BlogPagePreview;

                if (preview != null)
                {
                    var BlogPagePreview = await GetBlogPagePreview(preview);

                    db.BlogPages.Add(BlogPagePreview);
                    await db.SaveChangesAsync();

                    return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new AjaxResponse { Success = false, Message = result.Value }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ApplyViewNews(int? idNews, int? idBlog)
        {
            var result = await Utils.PrepareDataPage("NewsPage", "~/Content/rsc/imgs", "PreviewNews", new NewsPagePreview { NewsPageName = "PreviewNews", NewsPageId = idNews }, FileType.Image, FillDataTextNewsPage, FillDataImageNewsPage);

            if (result.Key)
            {
                PagePreview previewNews = await db.PagePreviews.FindAsync("PreviewNews");

                var str = new MemoryStream(previewNews.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var preview = binaryFormatter.Deserialize(str) as NewsPagePreview;

                if (preview != null)
                {
                    var NewsPageEdit = await GetNewsPagePreview(preview);
                    if (idNews == 0)
                    {
                        if (idBlog != null) NewsPageEdit.BlogPageId = idBlog.Value;
                        db.NewsPages.Add(NewsPageEdit);
                    }
                    else
                    {
                        db.Entry(NewsPageEdit).State = EntityState.Modified;
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

        private async Task<BlogPage> GetBlogPagePreview(BlogPagePreview preview)
        {
            var BlogPageActive = await GetActiveBlogPage(int.Parse(preview.TipoBlog));
            var lastIdBlogPage = await db.BlogPages.MaxAsync(x => x.BlogPageId) + 1;
            var lastIdNewsPage = await db.NewsPages.MaxAsync(x => x.NewsPageId);

            var idtipoBlog = int.Parse(preview.TipoBlog);
            var tipoBlog = await db.BlogTypes.FirstOrDefaultAsync(x=>x.BlogTypeId == idtipoBlog);
            if (tipoBlog != null)
            {
                var blogPagePreview = new BlogPage
                {
                    BlogPageCustomValue = $"BlogPage_{tipoBlog.BlogTypeName.Trim()}_{lastIdBlogPage}",
                    BlogPageTitle = "Blog",
                    BlogPageActive = "EnEdicion",
                    BlogPageCreatedDate = DateTime.Now,
                    BlogPageImage = preview.ImageBlogPage ?? BlogPageActive.BlogPageImage,
                    BlogTypeId = idtipoBlog,
                    BlogPageColorBgHead = preview.ColorBgHeadBlogPage ?? BlogPageActive.BlogPageColorBgHead,
                    BlogPageColorTextHead = preview.ColorTextHeadBlogPage ?? BlogPageActive.BlogPageColorTextHead,
                    BlogPageTextDesc = preview.TextDescBlogPage ?? BlogPageActive.BlogPageTextDesc,
                    BlogPageTitleDesc = preview.TitleDescBlogPage ?? BlogPageActive.BlogPageTitleDesc,
                    BlogPageColorTitleDesc = preview.ColorTitleDescBlogPage ?? BlogPageActive.BlogPageColorTitleDesc,
                    BlogPageTextHead = preview.TextHeadBlogPage ?? BlogPageActive.BlogPageTextHead,
                    BlogPageColorBgTitleDesc = preview.ColorBgTitleDescBlogPage ?? BlogPageActive.BlogPageColorBgTitleDesc,
                    BlogPageColorTextDescHead = preview.ColorTextDescHeadBlogPage ?? BlogPageActive.BlogPageColorTextDescHead,
                    BlogTypes = db.BlogTypes.ToList(),
                    News = new List<NewsPage>()
                };

                for (int i = 0; i < BlogPageActive.News.Count; i++)
                {
                    blogPagePreview.News.Add(new NewsPage
                    {
                        NewsPageActive = "EnEdicion",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageCustomValue = $"News_{++lastIdNewsPage}",
                        NewsPageImageHead = BlogPageActive.News[i].NewsPageImageHead,
                        NewsPageOrder = i + 1,
                        NewsPageTitle = "Noticias",
                        NewsPageImageDescription = BlogPageActive.News[i].NewsPageImageDescription,
                        NewsPageColorBgHead = BlogPageActive.News[i].NewsPageColorBgHead,
                        NewsPageColorBgSubTextHead = BlogPageActive.News[i].NewsPageColorBgSubTextHead,
                        NewsPageColorSubTextHead = BlogPageActive.News[i].NewsPageColorSubTextHead,
                        NewsPageColorTextHead = BlogPageActive.News[i].NewsPageColorTextHead,
                        NewsPageSubTextHead = BlogPageActive.News[i].NewsPageSubTextHead,
                        NewsPageTextHead = BlogPageActive.News[i].NewsPageTextHead,
                        NewsPageTitleDescription1 = BlogPageActive.News[i].NewsPageTitleDescription1,
                        NewsPageColorTitleDescription1 = BlogPageActive.News[i].NewsPageColorTitleDescription1,
                        NewsPageTitleDescription2 = BlogPageActive.News[i].NewsPageTitleDescription2,
                        NewsPageColorTitleDescription2 = BlogPageActive.News[i].NewsPageColorTitleDescription2,
                        NewsPageTextDescription1 = BlogPageActive.News[i].NewsPageTextDescription1,
                        NewsPageTextDescription2 = BlogPageActive.News[i].NewsPageTextDescription2,
                    });
                }

                return blogPagePreview;
            }
            return null;
        }

        private async Task<NewsPage> GetNewsPagePreview(NewsPagePreview preview)
        {
            NewsPage news;
            if (preview.NewsPageId == 0)
            {
                var lastIdNews = await db.NewsPages.MaxAsync(x => x.NewsPageId) + 1;
                var lastProductOrder = await db.NewsPages.Where(x => x.BlogPageId == preview.BlogPageId).ToListAsync();
                int maxOrder = 1;
                if (lastProductOrder.Count > 0)
                {
                    maxOrder = lastProductOrder.Max(x => x.NewsPageOrder) + 1;
                }

                news = new NewsPage
                {
                    NewsPageTitle = "Noticias",
                    NewsPageActive = "EnEdicion",
                    NewsPageCreatedDate = DateTime.Now,
                    NewsPageCustomValue = $"News_{++lastIdNews}",
                    NewsPageImageHead = preview.ImageHeadNewsPage,
                    NewsPageImageDescription = preview.ImageDescriptionNewsPage,
                    NewsPageColorBgHead = preview.ColorBgHeadNewsPage,
                    NewsPageColorBgSubTextHead = preview.ColorBgSubTextHeadNewsPage,
                    NewsPageColorSubTextHead = preview.ColorSubTextHeadNewsPage,
                    NewsPageColorTextHead = preview.ColorTextHeadNewsPage,
                    NewsPageSubTextHead = preview.SubTextHeadNewsPage,
                    NewsPageTextHead = preview.TextHeadNewsPage,
                    NewsPageTitleDescription1 = preview.TitleDescription1NewsPage,
                    NewsPageColorTitleDescription1 = preview.ColorTitleDescription1NewsPage,
                    NewsPageTitleDescription2 = preview.TitleDescription2NewsPage,
                    NewsPageColorTitleDescription2 = preview.ColorTitleDescription2NewsPage,
                    NewsPageTextDescription1 = preview.TextDescription1NewsPage,
                    NewsPageTextDescription2 = preview.TextDescription2NewsPage,
                    NewsPageOrder = maxOrder,
                };
            }
            else
            {
                news = await db.NewsPages.FirstAsync(x => x.NewsPageId == preview.NewsPageId);
                if(news != null)
                {
                    news.NewsPageImageHead = preview.ImageHeadNewsPage;
                    news.NewsPageImageDescription = preview.ImageDescriptionNewsPage;
                    news.NewsPageColorBgHead = preview.ColorBgHeadNewsPage;
                    news.NewsPageColorBgSubTextHead = preview.ColorBgSubTextHeadNewsPage;
                    news.NewsPageColorSubTextHead = preview.ColorSubTextHeadNewsPage;
                    news.NewsPageColorTextHead = preview.ColorTextHeadNewsPage;
                    news.NewsPageSubTextHead = preview.SubTextHeadNewsPage;
                    news.NewsPageTextHead = preview.TextHeadNewsPage;
                    news.NewsPageTitleDescription1 = preview.TitleDescription1NewsPage;
                    news.NewsPageColorTitleDescription1 = preview.ColorTitleDescription1NewsPage;
                    news.NewsPageTitleDescription2 = preview.TitleDescription2NewsPage;
                    news.NewsPageColorTitleDescription2 = preview.ColorTitleDescription2NewsPage;
                    news.NewsPageTextDescription1 = preview.TextDescription1NewsPage;
                    news.NewsPageTextDescription2 = preview.TextDescription2NewsPage;
                }
            }

            return news;
        }

        private async Task<BlogPage> GetActiveBlogPage(int type)
        {
            var h = await db.BlogPages.FirstOrDefaultAsync(p => p.BlogPageActive == "Activada" && p.BlogTypeId == type);
            if (h != null)
            {
                h.BlogTypes = await db.BlogTypes.ToListAsync();

                h.News = h.News.OrderBy(x => x.NewsPageOrder).ToList();

                return h;
            }
            return null;            
        }

        private static void FillDataImageBlogPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as BlogPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImageBlogPage":
                        preview.ImageBlogPage = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextBlogPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as BlogPagePreview;
            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "TipoBlog":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TipoBlog = formValue;
                        break;
                    case "ColorBgHeadBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorBgHeadBlogPage = formValue;
                        break;
                    case "ColorTextHeadBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTextHeadBlogPage = formValue;
                        break;
                    case "TextHeadBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextHeadBlogPage = formValue;
                        break;
                    case "ColorTextDescHeadBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTextDescHeadBlogPage = formValue;
                        break;
                    case "TitleDescBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TitleDescBlogPage = formValue;
                        break;
                    case "ColorTitleDescBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTitleDescBlogPage = formValue;
                        break;
                    case "ColorBgTitleDescBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorBgTitleDescBlogPage = formValue;
                        break;
                    case "TextDescBlogPage":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextDescBlogPage = formValue;
                        break;
                    case "ImageBlogPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        if (formValue != null)
                            preview.ImageBlogPage = "~/" + formValue.Substring(formValue.IndexOf("Content", StringComparison.Ordinal));
                        break;
                }
            }
        }

        private static void FillDataImageNewsPage(string fileName, string file, IPagePreview preview1)
        {
            var preview = preview1 as NewsPagePreview;
            if (preview != null)
            {
                switch (file)
                {
                    case "ImageHeadNewsPage":
                        preview.ImageHeadNewsPage = fileName;
                        break;
                    case "ImageDescriptionNewsPage":
                        preview.ImageDescriptionNewsPage = fileName;
                        break;
                }
            }
        }

        private static void FillDataTextNewsPage(HttpRequest httpRequest, string form, IPagePreview preview1)
        {
            var preview = preview1 as NewsPagePreview;
            if (preview != null)
            {
                string formValue;
                switch (form)
                {
                    case "ColorBgHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorBgHeadNewsPage = formValue;
                        break;
                    case "TextHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextHeadNewsPage = formValue;
                        break;

                    case "ColorTextHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTextHeadNewsPage = formValue;
                        break;

                    case "SubTextHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.SubTextHeadNewsPage = formValue;
                        break;

                    case "ColorSubTextHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorSubTextHeadNewsPage = formValue;
                        break;

                    case "ColorBgSubTextHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorBgSubTextHeadNewsPage = formValue;
                        break;

                    case "TitleDescription1NewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TitleDescription1NewsPage = formValue;
                        break;

                    case "TitleDescription2NewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TitleDescription2NewsPage = formValue;
                        break;

                    case "ColorTitleDescription1NewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTitleDescription1NewsPage = formValue;
                        break;

                    case "ColorTitleDescription2NewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.ColorTitleDescription2NewsPage = formValue;
                        break;
                    case "TextDescription1NewsPage":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextDescription1NewsPage = formValue;
                        break;
                    case "TextDescription2NewsPage":
                        formValue = httpRequest.Unvalidated.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        preview.TextDescription2NewsPage = formValue;
                        break;
                    //case "ColorTextDescription1NewsPage":
                    //    formValue = httpRequest.Form[form];
                    //    formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                    //    preview.ColorTextDescription1NewsPage = formValue;
                    //    break;
                    //case "ColorTextDescription2NewsPage":
                    //    formValue = httpRequest.Form[form];
                    //    formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                    //    preview.ColorTextDescription2NewsPage = formValue;
                    //    break;

                    case "ImageHeadNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                        if(formValue != null)
                            preview.ImageHeadNewsPage = "~/" + formValue.Substring(formValue.IndexOf("Content", StringComparison.Ordinal));
                        break;
                    case "ImageDescriptionNewsPage":
                        formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) || formValue == "undefined" ? null : formValue;
                        if (formValue != null)
                            preview.ImageDescriptionNewsPage = "~/" + formValue.Substring(formValue.IndexOf("Content", StringComparison.Ordinal));
                        break;
                }
            }
        }
    }
}
