using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Privacidad
    /// </summary>
    public class PrivacityPage
    {
        public int PrivacityPageId { get; set; }

        [Required]
        public string PrivacityPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool PrivacityPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime PrivacityPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string PrivacityPageCustomValue { get; set; }

        public string PrivacityPageHeadText { get; set; }

        public string PrivacityPageColorHeadText { get; set; }

        public string PrivacityPageBgColorHead { get; set; }

        public string PrivacityPageTextDescription { get; set; }

        public string PrivacityPageTextColor { get; set; }

        public string PrivacityPageTextTitle { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Privacidad
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class PrivacityPagePreview : IPagePreview
    {
        public string PrivacityPageName { get; set; }

        public string HeadTextPrivacityPage { get; set; }

        public string ColorHeadTextPrivacityPage { get; set; }

        public string BgColorHeadPrivacityPage { get; set; }

        public string TextDescriptionPrivacityPage { get; set; }

        public string TextColorPrivacityPage { get; set; }

        public string TextTitlePrivacityPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
