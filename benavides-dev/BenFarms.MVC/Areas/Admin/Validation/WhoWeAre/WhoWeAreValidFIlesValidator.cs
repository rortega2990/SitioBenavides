using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Sections;


namespace BenFarms.MVC.Areas.Admin.Validation.WhoWeAre
{
    public class WhoWeAreValidFilesInCollectionValidator : InputModelValidator<WhoWeArePage, WhoWeArePage>
    {
        private readonly List<WhoWeAreTitledSection> section;
        private readonly List<WhoWeAreTitledSection> targetSection;
        private readonly bool ignoreMissingFiles;

        public WhoWeAreValidFilesInCollectionValidator(List<WhoWeAreTitledSection> section, List<WhoWeAreTitledSection> targetSection, 
                                                        bool ignoreMissingFiles)
        {
            this.section = section;
            this.targetSection = targetSection;
            this.ignoreMissingFiles = ignoreMissingFiles;
        }
        public override List<string> brokenRules(WhoWeArePage inputModel, WhoWeArePage referencePage, IEnumerable<string> uploadedFiles)
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
        }
    }
}