using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Reporte
    /// </summary>
    public class ReportType
    {
        public int ReportTypeId { get; set; }

        [Display(Name ="Nombre del Reporte")]
        public string ReportName { get; set; }

        [Display(Name = "Descripción del Reporte")]
        public string ReportDescription{ get; set; }

        [Display(Name = "Estado")]
        public bool ReportActive { get; set; }

        public virtual List<ReportFiles> ReportFiles { get; set; }

        public List<int> Years()
        {
            return ReportFiles?.Select(x => x.Year).Distinct().ToList() ?? new List<int>();
        }

        public List<ReportFiles> Files(int year)
        {
            return ReportFiles?.Where(x => x.Year == year).ToList() ?? new List<ReportFiles>();
        }
    }
}