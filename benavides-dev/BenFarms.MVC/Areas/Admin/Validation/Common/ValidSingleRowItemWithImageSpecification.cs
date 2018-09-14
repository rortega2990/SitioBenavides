using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages.Sections;

namespace BenFarms.MVC.Areas.Admin.Validation.Common
{
    public class ValidSingleRowItemWithImageSpecification : Specification<WhoWeAreSimpleRowItemWithImage>
    {
        private readonly IEnumerable<string> files;
        private readonly bool ignoreMissingFile;

        public ValidSingleRowItemWithImageSpecification(IEnumerable<string> uploadedFiles, bool ignoreMissingFile)
        {
            this.files = uploadedFiles;
            this.ignoreMissingFile = ignoreMissingFile;
        }
        public override List<string> brokenRules(WhoWeAreSimpleRowItemWithImage entity)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(entity.Title) == false)
            {
                if (string.IsNullOrEmpty(entity.TitleTextColor) == true)
                {
                    result.Add("Color no válido para el título: " + entity.Title);
                }
            }

            if (string.IsNullOrEmpty(entity.Message) == false)
            {
                if (string.IsNullOrEmpty(entity.MessageTextColor) == true)
                {
                    result.Add("Color no válido para el mensaje: " + entity.Message);
                }
            }

            var file = files.Where(f => f == entity.ImageFileName).FirstOrDefault();

            if(String.IsNullOrEmpty(file) == true && this.ignoreMissingFile == false)
            {
                result.Add("Fichero no encontrado: " + entity.ImageFileName);
            }

            return result;
        }
    }
}