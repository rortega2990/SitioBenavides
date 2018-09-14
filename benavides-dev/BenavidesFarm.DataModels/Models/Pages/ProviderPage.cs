using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Proveedores
    /// </summary>
    public class ProviderPage
    {
        public int ProviderPageId { get; set; }

        [Required]
        public string ProviderPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool ProviderPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime ProviderPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string ProviderPageCustomValue { get; set; }

        public string ProviderPageHeadText { get; set; }

        public string ProviderPageColorHeadText { get; set; }

        public string ProviderPageBgColorHead { get; set; }

        public string ProviderPageSubText { get; set; }

        public string ProviderPageColorSubText { get; set; }

        [NotMapped]
        public List<DocumentType> DocumentTypes { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Proveedores
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class ProviderPagePreview : IPagePreview
    {
        public string ProviderPageName { get; set; }

        public string HeadTextProviderPage { get; set; }

        public string ColorHeadTextProviderPage { get; set; }

        public string BgColorHeadProviderPage { get; set; }

        public string SubText1ProviderPage { get; set; }

        public string ColorSubTextProviderPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
