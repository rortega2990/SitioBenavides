using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class LaboratoryViewModel: LaboratoryInputModel
    {
        public string City { get; set; }
        public string State { get; set; }
    }
}