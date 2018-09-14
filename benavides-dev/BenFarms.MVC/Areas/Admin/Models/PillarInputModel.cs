using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class PillarInputModel
    {
        [Display(Name = "Id")]
        public int PillarId { get; set; }

        [Display(Name = "Nombre")]
        public string PillarName { get; set; }
        
        [Display(Name = "Estado")]
        public bool PillarActive { get; set; }

        [Display(Name = "Descripción")]
        public string PillarDescription { get; set; }

        [Display(Name = "Enlace")]
        public string PillarLink { get; set; }

        [Display(Name = "Imagen")]
        public HttpFileCollectionBase PillarImage { get; set; }
    }
}