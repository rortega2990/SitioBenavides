using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation.DoctorsOffice
{
    public class DoctorsOfficeServiceSectionValidator : Specification<DoctorsOfficePageSection>
    {
        public override List<string> brokenRules(DoctorsOfficePageSection entity)
        {
            List<string> result = new List<string>();

            if(String.IsNullOrEmpty(entity.Title))
            {
                result.Add("El título de la sección de servicios no puede ser vacío");
            }

            if(String.IsNullOrEmpty(entity.TitleColor))
            {
                result.Add("El color del título de la sección de servicios no puede ser vacío");
            }

            return result;
        }
    }
}