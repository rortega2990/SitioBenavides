using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Areas.Admin.Services;
using BenFarms.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorsOfficeController : Controller
    {
        private readonly MyApplicationDbContext context;
        private readonly DoctorsOfficeService service;

        public DoctorsOfficeController()
        {
            context = new MyApplicationDbContext();
            service = new DoctorsOfficeService(context);
        }

        // GET: Admin/DoctorsOffice
        public ActionResult Index()
        {
            var allDoctorsOfficePages = this.context.DoctorsOfficePages.ToList();
            return View(allDoctorsOfficePages);
        }
        public ActionResult Edit(int? id)
        {
            var model = service.getPage(id);

            if (model == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(model);
        }

        public ActionResult SetState(int? id)
        {
            var operationResult = service.ActivatePage(id);
            if (operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult AddOrEdit(DoctorsOfficePage page)
        {
            HttpFileCollectionBase files = Request.Files;
            
            var operationResult = service.CreateOrUpdate(new DoctorsOfficeInputModel() { page = page, uploadedFiles = Request.Files }, true);
            if (operationResult.IsValid)
            {
                return Json(new AjaxResponse { Success = true, Message = "Los cambios se aplicaron correctamente en el servidor" }, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = "";
            operationResult.Errors.ForEach(e => errorMessage += ("\n" + e));

            return Json(new AjaxResponse { Success = false, Message = errorMessage }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult PreviewPage(DoctorsOfficePage page)
        {
            var operationResult = service.PreviewPage(new DoctorsOfficeInputModel() { page = page, uploadedFiles = Request.Files });

            if (operationResult.IsValid)
            {
                return Json(new AjaxResponse { Success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            string errorMessage = "";
            operationResult.Errors.ForEach(e => errorMessage += ("\n" + e));
            return Json(new AjaxResponse { Success = true, Message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreviewEdit()
        {
            var preview = service.GetPreviewPage();

            if (preview == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View("Preview", preview.ResultObject);
        }

        public ActionResult PreviewPageById(int? id)
        {
            var page = service.getPage(id);
            if (page != null)
            {
                return View("Preview", page);
            }
            return RedirectToAction("NotFound", "Error");
        }

        public ActionResult ConfirmDeletion(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

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
            var operationResult = service.DeletePage(id);

            if (operationResult.IsValid)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFound", "Error");
        }

    }
}