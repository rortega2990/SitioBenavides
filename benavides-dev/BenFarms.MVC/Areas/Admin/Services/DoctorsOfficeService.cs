using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Areas.Admin.Validation.DoctorsOffice;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Services
{

    public class DoctorsOfficeService
    {
        private readonly MyApplicationDbContext context;
        public DoctorsOfficeService(MyApplicationDbContext context)
        {
            this.context = context;
        }

        public DoctorsOfficePage getPage(int? id)
        {
            DoctorsOfficePage result = null;

            var pagesQueryable = context.DoctorsOfficePages.AsQueryable();
            if (id != null && id > 0)
            {
                pagesQueryable = pagesQueryable.Where(p => p.Id == id);

            }
            else
            {
                pagesQueryable = context.DoctorsOfficePages.Where(p => p.Active == true);
            }

            result = pagesQueryable.Include(p => p.HeadImages)
                            .Include(p => p.ServicesSection)
                            .FirstOrDefault();

            return result;


        }

        public DoctorsOfficePage getActivePage()
        {
            DoctorsOfficePage result = null;

            result = context.DoctorsOfficePages.Where(p => p.Active == true)
                    .Include(p => p.HeadImages)
                    .Include(p => p.ServicesSection)
                    .FirstOrDefault();

            return result;

        }

        public AdministrationServiceResult PreviewPage(DoctorsOfficeInputModel inputModel)
        {
            AdministrationServiceResult result;

            var referencePage = this.getPage(inputModel.page.Id);

            if (referencePage != null)
            {
                result = CreatePageFromReference(inputModel, referencePage);
                if (result.IsValid == true)
                {
                    WhoWeArePage page = result.ResultObject as WhoWeArePage;
                    result.ResultObject = page;

                    IFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    formatter.Serialize(stream, page);
                    stream.Close();

                    var storedPagePreview = context.PagePreviews.Find("DoctorsOffice");
                    if (storedPagePreview != null)
                    {
                        context.PagePreviews.Remove(storedPagePreview);
                    }

                    context.PagePreviews.Add(new PagePreview { PageName = "DoctorsOffice", PageValue = stream.GetBuffer() });
                    context.SaveChanges();

                }
            }
            else
            {
                result = new AdministrationServiceResult();
                result.Errors.Add("Pagina no válida");
            }
            return result;
        }

        public AdministrationServiceResult GetPreviewPage()
        {
            var result = new AdministrationServiceResult();
            var preview = context.PagePreviews.Find("DoctorsOffice");

            if (preview == null)
            {
                result.Errors.Add("Página no válida");
            }
            else
            {
                var str = new MemoryStream(preview.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var previewPage = binaryFormatter.Deserialize(str) as DoctorsOfficePage;
                result.ResultObject = previewPage;
            }

            return result;
        }

        public AdministrationServiceResult CreateOrUpdate(DoctorsOfficeInputModel inputModel, bool setAsActivePage = false)
        {
            AdministrationServiceResult result;

            var referencePage = this.getPage(inputModel.page.Id);

            if (referencePage != null)
            {
                result = CreatePageFromReference(inputModel, referencePage);
                if (result.IsValid == true)
                {
                    DoctorsOfficePage page = result.ResultObject as DoctorsOfficePage;
                    page.Active = false;
                    context.DoctorsOfficePages.Add(page);
                    context.SaveChanges();

                    if (setAsActivePage == true)
                    {
                        this.ActivatePage(page, referencePage);
                    }
                }
            }
            else
            {
                result = new AdministrationServiceResult();
                result.Errors.Add("Pagina no válida");
            }
            return result;
        }


        public AdministrationServiceResult ActivatePage(int? id)
        {
            var activePage = getActivePage();
            var targetPage = getPage(id);
            return this.ActivatePage(targetPage, activePage);
        }

        public AdministrationServiceResult DeletePage(int? id)
        {
            var result = new AdministrationServiceResult();

            var page = this.getPage(id);

            if (page == null)
            {
                result.Errors.Add("Página no encontrada");
            }
            else
            {
                this.context.DoctorsOfficePages.Remove(page);
                this.context.SaveChanges();
            }

            return result;
        }

        private AdministrationServiceResult ActivatePage(DoctorsOfficePage targetPage, DoctorsOfficePage activePage)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();

            if (activePage != null && targetPage != null)
            {
                activePage.Active = false;
                targetPage.Active = true;
                context.SaveChanges();
            }
            else
            {
                result.Errors.Add("Parámetros no válidos");
            }

            return result;
        }

        private AdministrationServiceResult CreatePageFromReference(DoctorsOfficeInputModel inputModel, DoctorsOfficePage referencePage)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();
            var specification = new DoctorsOfficeValidInputModelSpecification(referencePage);
            var brokenRules = specification.brokenRules(inputModel);
            result.Errors.AddRange(brokenRules);

            if (result.IsValid)
            {
                var page = inputModel.page;
                if (page.Equals(referencePage) == false)
                {
                    var fileService = new FileService();
                    var processedFIles = fileService.processFiles(inputModel.uploadedFiles, "~/UploadedFiles");
                    fileService.mapFilesToSectionForCreateOrUpdate(page.HeadImages, referencePage.HeadImages, processedFIles);
                    fileService.mapFilesToSectionForCreateOrUpdate(page.ServicesSection, referencePage.ServicesSection, processedFIles);
                    page.CreationDate = DateTime.Now;
                    result.ResultObject = page;
                }
                else
                {
                    result.Errors.Add("Esta página ya se encuentra en el catálogo");
                }
            }
            return result;
        }




    }
}