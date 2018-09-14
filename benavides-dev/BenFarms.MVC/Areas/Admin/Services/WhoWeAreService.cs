using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Areas.Admin.Models;
using System.Data.Entity;
using BenFarms.MVC.Areas.Admin.Validation.WhoWeAre;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BenFarms.MVC.Areas.Admin.Services
{
    public class WhoWeAreService
    {
        private readonly MyApplicationDbContext context;
        public WhoWeAreService(MyApplicationDbContext context)
        {
            this.context = context;
        }

        public WhoWeArePage getPage(int? id)
        {
            WhoWeArePage result = null;
            var pagesQueryable = context.WhoWeArePages.AsQueryable();
            if (id != null && id > 0)
            {
                pagesQueryable = pagesQueryable.Where(p => p.Id == id);

            }
            else
            {
                pagesQueryable = context.WhoWeArePages.Where(p => p.Active == true);
            }

            result = pagesQueryable.Include(p => p.HistoryImages)
                            .Include(p => p.HeadImages)
                            .Include(p => p.ValuesSection)
                            .Include(p => p.AdSection)
                            .FirstOrDefault();

            if(result != null)
            {
                result.HistoryImages.Sort();
            }

            return result;
        }

        public WhoWeArePage getActivePage()
        {
            var page = context.WhoWeArePages.Where(p => p.Active == true).Include(p => p.HistoryImages)
                            .Include(p => p.HeadImages)
                            .Include(p => p.ValuesSection)
                            .Include(p => p.AdSection)
                            .FirstOrDefault();

            if(page != null)
            {
                page.HistoryImages.Sort();
            }

            return page;
        }

        public AdministrationServiceResult PreviewPage(WhoWeAreInputModel inputModel)
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

                    var storedPagePreview = context.PagePreviews.Find("WhoWeAre");
                    if (storedPagePreview != null)
                    {
                        context.PagePreviews.Remove(storedPagePreview);
                    }

                    context.PagePreviews.Add(new PagePreview { PageName = "WhoWeAre", PageValue = stream.GetBuffer() });
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
            var preview = context.PagePreviews.Find("WhoWeAre");

            if (preview == null)
            {
                result.Errors.Add("Página no válida");
            }
            else
            {
                var str = new MemoryStream(preview.PageValue);
                var binaryFormatter = new BinaryFormatter();
                var previewPage = binaryFormatter.Deserialize(str) as WhoWeArePage;

                previewPage.HistoryImages.Sort();

                result.ResultObject = previewPage;
            }
            
            return result;
        }

        public  AdministrationServiceResult CreateOrUpdate(WhoWeAreInputModel inputModel, bool setAsActivePage = false)
        {
            AdministrationServiceResult result;

            var referencePage = this.getPage(inputModel.page.Id);

            if(referencePage != null)
            {
                result = CreatePageFromReference(inputModel, referencePage);
                if (result.IsValid == true)
                {
                    WhoWeArePage page = result.ResultObject as WhoWeArePage;
                    page.Active = false;
                    context.WhoWeArePages.Add(page);
                    context.SaveChanges();

                    if(setAsActivePage == true)
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

            if(page == null)
            {
                result.Errors.Add("Página no encontrada");
            }
            else
            {
                this.context.WhoWeArePages.Remove(page);
                this.context.SaveChanges();
            }

            return result;
        }

        private AdministrationServiceResult ActivatePage(WhoWeArePage targetPage, WhoWeArePage activePage)
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

        private AdministrationServiceResult CreatePageFromReference(WhoWeAreInputModel inputModel, WhoWeArePage referencePage)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();
            var specification = new WhoWeAreValidInputModelWIthReferencePageSpecification(referencePage);
            var brokenRules = specification.brokenRules(inputModel);
            result.Errors.AddRange(brokenRules);

            if(result.IsValid)
            {
                var page = inputModel.page;
                if (page.Equals(referencePage) == false)
                {

                    var fileService = new FileService();
                    var processedFIles = fileService.processFiles(inputModel.uploadedFiles, "~/UploadedFiles");
                    fileService.mapFilesToSectionForCreateOrUpdate(page.HeadImages, referencePage.HeadImages, processedFIles);
                    fileService.mapFilesToSectionForCreateOrUpdate(page.ValuesSection, referencePage.ValuesSection, processedFIles);
                    fileService.mapFilesToSectionForCreateOrUpdate(page.HistoryImages, referencePage.HistoryImages, processedFIles);
                    var adFileName = page.AdSection.ImageFileName;
                    page.AdSection.ImageFileName = (String.IsNullOrEmpty(adFileName) == false && processedFIles.ContainsKey(adFileName)) ? processedFIles[adFileName] : referencePage.AdSection.ImageFileName;
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