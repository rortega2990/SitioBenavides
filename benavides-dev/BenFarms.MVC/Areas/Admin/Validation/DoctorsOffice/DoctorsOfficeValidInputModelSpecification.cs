using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Areas.Admin.Validation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation.DoctorsOffice
{
    public class DoctorsOfficeValidInputModelSpecification : Specification<DoctorsOfficeInputModel>
    {
        //private readonly DoctorsOfficeInputModel inputModel;
        private readonly DoctorsOfficePage referencePage;
        public DoctorsOfficeValidInputModelSpecification(DoctorsOfficePage referencePage)
        {
            this.referencePage = referencePage;
        }
        public override List<string> brokenRules(DoctorsOfficeInputModel inputModel)
        {
            List<string> result = new List<string>();
            var incommingPage = inputModel.page;
            var uploadedFiles = inputModel.uploadedFiles.AllKeys;

            var headImagesFileValidator = new ValidFilesInCollectionValidator(referencePage.HeadImages, uploadedFiles,false);
            var servicesSectionFileValidator = new ValidFilesInCollectionValidator(referencePage.HeadImages, uploadedFiles, false);

            result.AddRange(headImagesFileValidator.brokenRules(incommingPage.HeadImages));
            result.AddRange(headImagesFileValidator.brokenRules(incommingPage.ServicesSection));

            var servicesSectionContentValidator = new CompositeObjectSpecification<DoctorsOfficePageSection>(referencePage.ServicesSection);
            result.AddRange(servicesSectionContentValidator.brokenRules(new DoctorsOfficeServiceSectionValidator()));

            return result;
        }
    }
}