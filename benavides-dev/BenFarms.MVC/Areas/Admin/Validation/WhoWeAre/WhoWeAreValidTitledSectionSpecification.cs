using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation.WhoWeAre
{
    public class WhoWeAreValidTitledSectionSpecification : Specification<WhoWeAreTitledSection>
    {
        private readonly IEnumerable<string> files;
        private readonly bool ignoreMissingFile;

        public WhoWeAreValidTitledSectionSpecification(IEnumerable<string> uploadedFiles, bool ignoreMissingFile)
        {
            this.files = uploadedFiles;
            this.ignoreMissingFile = ignoreMissingFile;
        }
        public override List<string> brokenRules(WhoWeAreTitledSection entity)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(entity.Title) == false)
            {
                if (string.IsNullOrEmpty(entity.TileColor) == true)
                {
                    result.Add("Color no válido para el título: " + entity.Title);
                }
            }

            if (string.IsNullOrEmpty(entity.Text) == false)
            {
                if (string.IsNullOrEmpty(entity. TextColor) == true)
                {
                    result.Add("Color no válido para el mensaje: " + entity.Text);
                }
            }

            if (string.IsNullOrEmpty(entity.SubTitle) == false)
            {
                if (string.IsNullOrEmpty(entity.SubTitleColor) == true)
                {
                    result.Add("Color no válido para el mensaje: " + entity.SubTitleColor);
                }
            }

            var file = files.Where(f => f == entity.ImageFileName).FirstOrDefault();

            if (String.IsNullOrEmpty(file) == true && this.ignoreMissingFile == false)
            {
                result.Add("Fichero no encontrado: " + entity.ImageFileName);
            }

            return result;
        }
    }
}