using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Términos y Condiciones
    /// </summary>
    public class ConditionsTermsPage
    {
        public int ConditionsTermsPageId { get; set; }

        [Required]
        public string ConditionsTermsPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool ConditionsTermsPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime ConditionsTermsPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string ConditionsTermsPageCustomValue { get; set; }

        public string ConditionsTermsPageHeadText { get; set; }

        public string ConditionsTermsPageColorHeadText { get; set; }

        public string ConditionsTermsPageBgColorHead { get; set; }

        public string ConditionsTermsPageTextDescription { get; set; }

        public string ConditionsTermsPageTextColor { get; set; }

        public string ConditionsTermsPageTextTitle { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Términos y Condiciones
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class ConditionsTermsPagePreview : IPagePreview
    {
        public string ConditionsTermsPageName { get; set; }

        public string HeadTextConditionsTermsPage { get; set; }

        public string ColorHeadTextConditionsTermsPage { get; set; }

        public string BgColorHeadConditionsTermsPage { get; set; }

        public string TextDescriptionConditionsTermsPage { get; set; }

        public string TextColorConditionsTermsPage { get; set; }

        public string TextTitleConditionsTermsPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
