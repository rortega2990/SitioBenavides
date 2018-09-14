using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class BranchInputModel
    {
        [Display(Name = "Id")]
        public int BranchId { get; set; }

        [Display(Name = "Nombre")]
        public string BranchName { get; set; }

        [Display(Name = "Region")]
        public string BranchRegion { get; set; }

        [Display(Name = "Ceco")]
        public string BranchCeco { get; set; }

        [Display(Name = "SAP")]
        public string BranchSap { get; set; }

        [Display(Name = "Dirección")]
        public string BranchAddress { get; set; }

        [Display(Name = "Ciudad")]
        public string BranchCity { get; set; }

        [Display(Name = "Consultorio Médico")]
        public bool BranchConsult { get; set; }

        [Display(Name = "Horario")]
        public string BranchHour1 { get; set; }

        public string BranchHour2 { get; set; }

        [Display(Name = "Longitud")]
        public string BranchLongitude { get; set; }

        [Display(Name = "Latitud")]
        public string BranchLatitude { get; set; }

        [Display(Name = "Estado")]
        public bool BranchActive { get; set; }

        [Display(Name = "Estado")]
        public int State { get; set; }

        [Display(Name = "Municipio")]
        public int City { get; set; }

        [Display(Name = "24 Horas")]
        public bool BranchTwentyFourHours { get; set; }

        [Display(Name = "Fose")]
        public bool BranchFose { get; set; }
    }
}