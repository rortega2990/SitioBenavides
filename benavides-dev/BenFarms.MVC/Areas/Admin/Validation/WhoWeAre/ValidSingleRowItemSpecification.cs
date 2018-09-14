using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages.Sections;


namespace BenFarms.MVC.Areas.Admin.Validation.WhoWeAre
{
    public class ValidSingleRowItemSpecification : Specification<WhoWeAreSimpleRowItem>
    {
        public override List<string> brokenRules(WhoWeAreSimpleRowItem entity)
        {
            List<string> result = new List<string>();

            if(string.IsNullOrEmpty(entity.Title) == false)
            {
                if(string.IsNullOrEmpty(entity.TitleTextColor) == true)
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

            return result;
        }
    }
}