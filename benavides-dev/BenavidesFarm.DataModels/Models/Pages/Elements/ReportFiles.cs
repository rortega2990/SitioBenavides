using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de archivo de Reporte
    /// </summary>
    public class ReportFiles
    {
        public int ReportFilesId { get; set; }

        public int ReportTypeId { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Archivo")]
        public string AddressFile { get; set; }

        [Display(Name = "Nombre descriptivo del archivo")]
        public string DescriptionFile { get; set; }

        public string GetFileName => !string.IsNullOrEmpty(AddressFile) ? AddressFile.Replace("~/Reports/", "") : "";

        public ReportType ReportType { get; set; }
    }
}