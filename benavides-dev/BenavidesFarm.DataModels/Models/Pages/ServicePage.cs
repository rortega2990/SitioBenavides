using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Servicios
    /// </summary>
    public class ServicePage
    {
        public int ServicePageId { get; set; }

        [Required]
        public string ServicePageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool ServicePageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime ServicePageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string ServicePageCustomValue { get; set; }

        public string ServicePageImageLogo { get; set; }

        public string ServicePageHeadText1 { get; set; }

        public string ServicePageHeadSubText1 { get; set; }

        public string ServicePageColorHeadText1 { get; set; }

        public string ServicePageColorHeadSubText1 { get; set; }

        public string ServicePageColorHeadBg { get; set; }

        public string ServicePageSubText { get; set; }

        public string ServicePageSubTextDescription { get; set; }

        public string ServicePageColorSubText { get; set; }

        public string ServicePageColorSubTextDescription { get; set; }

        [NotMapped]
        public IList<ServiceType> ServiceTypes { get; set; }
        
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Servicios
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class ServicePagePreview : IPagePreview
    {
        public string ImagenEncabezadoLogoServicio { get; set; }

        public string Texto1Servicio { get; set; }

        public string Texto2Servicio { get; set; }

        public string ColorTexto1Servicio { get; set; }

        public string ColorTexto2Servicio { get; set; }

        public string ColorFondoSerivicio { get; set; }

        public string TextoTituloServicio { get; set; }

        public string TextDescripcionServicio { get; set; }

        public string ColorTextDescripcionServicio { get; set; }
        public string ColorTextoTituloServicio { get; set; }

        public string ServicePageName { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
