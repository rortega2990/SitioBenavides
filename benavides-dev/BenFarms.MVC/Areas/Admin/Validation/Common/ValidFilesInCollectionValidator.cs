using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Sections;


namespace BenFarms.MVC.Areas.Admin.Validation.Common
{
    public class ValidFilesInCollectionValidator : Specification<IEnumerable<ISectionWithImage>>//InputModelValidator<WhoWeArePage, WhoWeArePage>
    {
        private readonly IEnumerable<ISectionWithImage> targetSection;
        private readonly bool ignoreMissingFiles;
        private readonly IEnumerable<string> uploadedFiles;

        public ValidFilesInCollectionValidator(IEnumerable<ISectionWithImage> targetSection, IEnumerable<string> uploadedFiles,
                                               bool ignoreMissingFiles)
        {
            this.uploadedFiles = uploadedFiles;
            this.targetSection = targetSection;
            this.ignoreMissingFiles = ignoreMissingFiles;
        }
        /*public override List<string> brokenRules(WhoWeArePage inputModel, WhoWeArePage referencePage, IEnumerable<string> uploadedFiles)
        {
            List<string> result = new List<string>();         

            foreach(var item in section)
            {
                if(item.ImageFileName != null)
                {
                    var file = uploadedFiles.Where(f => f == item.ImageFileName).FirstOrDefault();
                    if (string.IsNullOrEmpty(file))
                    {
                        file = targetSection.Where(s => s.Id.ToString() == item.ImageFileName).Select(s => s.ImageFileName).FirstOrDefault();
                        if (String.IsNullOrEmpty(file) && this.ignoreMissingFiles == false)
                        {
                            result.Add("Fichero no encontrado " + item.ImageFileName);
                        }
                    }
                }
            }

            return result;
        }*/

        public override List<string> brokenRules(IEnumerable<ISectionWithImage> section)
        {
            List<string> result = new List<string>();

            foreach (var item in section)
            {
                if (item.ImageFileName != null)
                {
                    var file = uploadedFiles.Where(f => f == item.ImageFileName).FirstOrDefault();
                    if (string.IsNullOrEmpty(file))
                    {
                        file = targetSection.Where(s => s.Id.ToString() == item.ImageFileName).Select(s => s.ImageFileName).FirstOrDefault();
                        if (String.IsNullOrEmpty(file) && this.ignoreMissingFiles == false)
                        {
                            result.Add("Fichero no encontrado " + item.ImageFileName);
                        }
                    }
                }
            }

            return result;
        }
    }
}