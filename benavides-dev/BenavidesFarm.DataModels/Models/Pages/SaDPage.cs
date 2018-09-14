using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de SaD
    /// </summary>
    public class SaDPage
    {
        public int SaDPageId { get; set; }

        [Required]
        public string SaDPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool SaDPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime SaDPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string SaDPageCustomValue { get; set; }

        public string SaDPageHeadText1 { get; set; }

        public string SaDPageHeadText2 { get; set; }

        public string SaDPageHeadTextColor1 { get; set; }

        public string SaDPageHeadTextColor2 { get; set; }

        public string SaDPageImageBg { get; set; }

        public string SaDPageImageLogo { get; set; }

        public string SaDPageSubTextColor1 { get; set; }        

        public string SaDPageSubText1 { get; set; }

        public string SaDPageNumberPrincipalText { get; set; }

        public string SaDPageNumberPrincipalTextColor { get; set; }

        public string SaDPageNumberPrincipalBgColor { get; set; }

        [NotMapped]
        public virtual List<SaDTypeNumber> SaDTypeNumbers { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de SaD
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class SaDPagePreview : IPagePreview
    {
        public string Texto1SaD { get; set; }

        public string Texto2SaD { get; set; }

        public string ColorTexto1SaD { get; set; }

        public string ColorTexto2SaD { get; set; }

        public string ImageBgSaD { get; set; }

        public string ImagenLogoSaD { get; set; }

        public string TextoTituloSaD { get; set; }

        public string ColorTextoTituloSaD { get; set; }

        public string NumeroprincipalSaD { get; set; }

        public string ColorNumeroprincipalSaD { get; set; }

        public string ColorBgNumeroprincipalSaD { get; set; }

        public string SaDPageName { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
