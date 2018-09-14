using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using BenFarms.MVC.Areas.Admin.Models;
using BenFarms.MVC.Areas.Admin.Validation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BenFarms.MVC.Areas.Admin.Validation.WhoWeAre
{
    public class WhoWeAreValidInputModelWIthReferencePageSpecification: Specification<WhoWeAreInputModel>
    {
        //private readonly WhoWeAreInputModel inputModel;
        private readonly WhoWeArePage referencePage;
        public WhoWeAreValidInputModelWIthReferencePageSpecification(WhoWeArePage referencePage)
        {
            this.referencePage = referencePage;
        }

        public override List<string> brokenRules(WhoWeAreInputModel inputModel)
        {
            var incommingPage = inputModel.page;
            var uploadedFiles = inputModel.uploadedFiles.AllKeys;

            var headImagesFileValidator = new ValidFilesInCollectionValidator(referencePage.HeadImages, uploadedFiles, false);
            var historySectionFileValidator = new ValidFilesInCollectionValidator(referencePage.HistoryImages, uploadedFiles, false);
            var valuesSectionFileValidator = new ValidFilesInCollectionValidator(referencePage.ValuesSection, uploadedFiles, false);

            List<string> brokenRules = headImagesFileValidator.brokenRules(incommingPage.HeadImages);
            brokenRules.AddRange(historySectionFileValidator.brokenRules(incommingPage.HistoryImages));
            brokenRules.AddRange(valuesSectionFileValidator.brokenRules(incommingPage.ValuesSection));


            /*var validatorsCollection = new List<InputModelValidator<WhoWeArePage, WhoWeArePage>>() {
                 new ValidFilesInCollectionValidator(incommingPage.HeadImages, referencePage.HeadImages,false),
                 new ValidFilesInCollectionValidator(incommingPage.ValuesSection, referencePage.ValuesSection,false),
                 new ValidFilesInCollectionValidator(incommingPage.HistoryImages, referencePage.HistoryImages,false)};

            var fileValidators = new CompositeInputModelValidator<WhoWeArePage, WhoWeArePage>(validatorsCollection);*/


            var singleRowItemSpecification = new ValidSingleRowItemSpecification();
            var singleRowItemWIthImageSpecification = new ValidSingleRowItemWithImageSpecification(inputModel.uploadedFiles.AllKeys, true);
            var titledItemSpecification = new ValidTitledSectionSpecification(inputModel.uploadedFiles.AllKeys, true);

            

            var simpleRowsCompositeSpecification = new CompositeObjectSpecification<WhoWeAreSimpleRowItem>(new List<WhoWeAreSimpleRowItem>() {
                                                    incommingPage.WhoWeAreSection,
                                                    incommingPage.VisionSection,
                                                    incommingPage.MisionSection});
            brokenRules.AddRange(simpleRowsCompositeSpecification.brokenRules(singleRowItemSpecification));
            brokenRules.AddRange(singleRowItemWIthImageSpecification.brokenRules(incommingPage.AdSection));

            incommingPage.HeadImages.ForEach(h => brokenRules.AddRange(titledItemSpecification.brokenRules(h)));
            incommingPage.ValuesSection.ForEach(h => brokenRules.AddRange(titledItemSpecification.brokenRules(h)));
            incommingPage.HistoryImages.ForEach(h => brokenRules.AddRange(titledItemSpecification.brokenRules(h)));

            return brokenRules;
        }
    }
}