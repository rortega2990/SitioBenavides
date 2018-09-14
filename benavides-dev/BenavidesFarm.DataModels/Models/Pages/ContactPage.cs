using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Contactos
    /// </summary>
    public class ContactPage
    {
        public int ContactPageId { get; set; }

        [Required]
        public string ContactPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool ContactPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime ContactPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string ContactPageCustomValue { get; set; }

        [Display(Name = "Id")]
        public string ContactPageHeadText { get; set; }

        [Display(Name = "Id")]
        public string ContactPageColorHeadText { get; set; }

        [Display(Name = "Id")]
        public string ContactPageBgColorHead { get; set; }

        [Display(Name = "Id")]
        public string ContactPageSubText1 { get; set; }

        [Display(Name = "Id")]
        public string ContactPageSubText2 { get; set; }

        [Display(Name = "Id")]
        public string ContactPageColorSubText1 { get; set; }

        [Display(Name = "Id")]
        public string ContactPageColorSubText2 { get; set; }

        [Display(Name = "Id")]
        public string ContactPageAddress { get; set; }

        [Display(Name = "Id")]
        public string ContactPageTelAddress { get; set; }

        [Display(Name = "Id")]
        public string ContactPageTelSaD { get; set; }

        [Display(Name = "Id")]
        public string ContactPageEmailSaD { get; set; }

        [Display(Name = "Id")]
        public string ContactPageTelAaP { get; set; }

        [Display(Name = "Id")]
        public string ContactPageColorTextFooter { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Contactos
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class ContactPagePreview : IPagePreview
    {
        public string ContactPageName { get; set; }
        public string HeadTextContactPage { get; set; }
        public string ColorHeadTextContactPage { get; set; }
        public string SubText1ContactPage { get; set; }
        public string SubText2ContactPage { get; set; }
        public string AddressContactPage { get; set; }
        public string BgColorHeadContactPage { get; set; }
        public string ColorSubText1ContactPage { get; set; }
        public string ColorSubText2ContactPage { get; set; }
        public string ColorTextFooterContactPage { get; set; }
        public string EmailSaDContactPage { get; set; }
        public string TelAaPContactPage { get; set; }
        public string TelAddressContactPage { get; set; }
        public string TelSaDContactPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
