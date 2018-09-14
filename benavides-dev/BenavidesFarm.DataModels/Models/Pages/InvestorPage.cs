using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Inverisonistas
    /// </summary>
    public class InvestorPage
    {
        public int InvestorPageId { get; set; }

        [Required]
        public string InvestorPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool InvestorPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime InvestorPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string InvestorPageCustomValue { get; set; }

        public string InvestorPageHeadText { get; set; }

        public string InvestorPageColorHeadText { get; set; }

        public string InvestorPageColorHeadBg { get; set; }

        public string InvestorPageSubText { get; set; }

        public string InvestorPageColorSubText { get; set; }

        [NotMapped]
        public List<ReportType> ReportTypes { get; set; }
       
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Inversionistas
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class InvestorPagePreview : IPagePreview
    {
        public string InvestorPageName { get; set; }

        public string HeadTextInvestorPage { get; set; }

        public string ColorHeadTextInvestorPage { get; set; }

        public string ColorHeadBgInvestorPage { get; set; }

        public string SubTextInvestorPage { get; set; }

        public string ColorSubTextInvestorPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
