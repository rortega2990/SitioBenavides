using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Sucursal
    /// </summary>
    public class Branch
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

        [Display(Name = "24 Horas")]
        public bool BranchTwentyFourHours { get; set; }

        [Display(Name = "Fose")]
        public bool BranchFose { get; set; }

        [Display(Name = "Horario")]
        public string BranchHour1 { get; set; }

        public string BranchHour2 { get; set; }

        [Display(Name = "Longitud")]
        public string BranchLongitude { get; set; }

        [Display(Name = "Latitud")]
        public string BranchLatitude { get; set; }

        [Display(Name = "Estado")]
        public bool BranchActive { get; set; }

        public Estados State { get; set; }
        public Municipios City { get; set; }

        public string GetHour()
        {
            if(!string.IsNullOrEmpty(BranchHour1) && !string.IsNullOrEmpty(BranchHour2))
            {
                if(BranchHour1 == "12:00 AM" && BranchHour2 == "12:00 PM")
                {
                    return "24 Horas";
                }
                return BranchHour1 + " a " + BranchHour2;
            }
            return "";
        }
    }
}
