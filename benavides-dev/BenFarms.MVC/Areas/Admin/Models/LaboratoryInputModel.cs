using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class LaboratoryInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public bool Active { get; set; }
    }
}