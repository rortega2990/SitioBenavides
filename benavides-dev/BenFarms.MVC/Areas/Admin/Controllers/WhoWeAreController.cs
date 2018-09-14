using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using System.Data.Entity;
using BenFarms.MVC.Areas.Admin.Services;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WhoWeAreController : Controller
    {
        MyApplicationDbContext context;

        public WhoWeAreController()
        {
            context = new MyApplicationDbContext();
        }

        // GET: Admin/WhoWeAre
        public ActionResult Index()
        {
            var allWhoWeArePages = this.context.WhoWeArePages.ToList();
            return View(allWhoWeArePages);
        }

        public ActionResult Edit(int? id)
        {
           /*if(id == null)
            {
                return View(new WhoWeArePage() {
                    HeadImages = new List<BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection>()
                    {
                        new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection()
                        {
                            Title = "La singandinga",
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                        },
                        new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection()
                        {
                            Title = "La singandinga",
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                        }
                    },
                    WhoWeAreSection = new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreSimpleRowItem()
                    {
                        Title = "QS",
                    },
                    MisionSection = new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreSimpleRowItem()
                    {
                        Title = "Mision",
                    },
                    VisionSection = new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreSimpleRowItem()
                    {
                        Title = "Vision",
                    },
                    ValuesSection = new List<BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection>()
                    {
                        new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection()
                        {
                            Title = "perra",
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                        },
                        new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection()
                        {
                            Title = "perra",
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                        }
                    },

                    HistoryImages = new List<BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection>()
                    {
                        new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreTitledSection()
                        {
                            Title = "perra",
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                        }
                    },
                    AdSection = new BenavidesFarm.DataModels.Models.Pages.Sections.WhoWeAreSimpleRowItemWithImage()
                    {
                        Title = "raca raca Ad",
                        ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png"
                    }
               });
            }*/

            var service = new WhoWeAreService(context);
            var model = service.getPage(id);

            
            if (model == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(model);
        }

        public ActionResult SetState(int? id)
        {
            WhoWeAreService service = new WhoWeAreService(context);
            var operationResult = service.ActivatePage(id);
            if(operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFound", "Error");

        }

        public ActionResult AddOrEdit(WhoWeArePage page)
        {
            HttpFileCollectionBase files = Request.Files;
            WhoWeAreService service = new WhoWeAreService(context);
            var operationResult = service.CreateOrUpdate(new WhoWeAreInputModel() { page = page, uploadedFiles = Request.Files }, true);
            if(operationResult.IsValid)
            {
                return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = "";
            operationResult.Errors.ForEach(e => errorMessage += ("\n" + e));

            return Json(new AjaxResponse { Success = false, Message = errorMessage }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult PreviewPage(WhoWeArePage page)
        {
            

            WhoWeAreService service = new WhoWeAreService(context);
            var operationResult = service.PreviewPage(new WhoWeAreInputModel() { page = page, uploadedFiles = Request.Files });
            
            if(operationResult.IsValid)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = "";
            operationResult.Errors.ForEach(e => errorMessage += ("\n" + e));
            return Json(new AjaxResponse { Success = true, Message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreviewEdit()
        {
            var service = new WhoWeAreService(context);
            var preview = service.GetPreviewPage();

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View("Preview", preview.ResultObject);
        }

        public ActionResult PreviewPageById(int? id)
        {
            WhoWeAreService service = new WhoWeAreService(context);
            var page = service.getPage(id);
            if (page != null)
            {
                return View("Preview",page);
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult ConfirmDeletion(int? id)
        {
           
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            WhoWeAreService service = new WhoWeAreService(context);
            var page = service.getPage(id);

            if (page == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new ConfirmDeletionViewModel()
            {
                CreationDate = page.CreationDate,
                Id = page.Id,
                Active = page.Active,
                Controller = this.GetType().Name.Replace("Controller", ""),
                CustomValue = page.Id.ToString(),
                Title = "Eliminar página de quiénes somos"
            };
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            WhoWeAreService service = new WhoWeAreService(context);

            var operationResult = service.DeletePage(id);

            if(operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFound", "Error");
        }
    }
}