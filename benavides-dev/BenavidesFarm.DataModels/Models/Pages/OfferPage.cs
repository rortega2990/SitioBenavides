using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Ofertas
    /// </summary>
    public class OfferPage
    {
        public int OfferPageId { get; set; }

        [Required]
        public string OfferPageTitle { get; set; }

        public int OfferTypeId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool OfferPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime OfferPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string OfferPageCustomValue { get; set; }

        public string OfferPageText1 { get; set; }

        public string OfferPageColorText1 { get; set; }

        public string OfferPageSpan1 { get; set; }

        public string OfferPageColorSpan1 { get; set; }

        public string OfferPageTextType1 { get; set; }

        public string OfferPageText2 { get; set; }

        public string OfferPageColorText2 { get; set; }

        public string OfferPageSpan2 { get; set; }
        
        public string OfferPageColorSpan2 { get; set; }

        public string OfferPageTextType2 { get; set; }

        public string OfferPageText3 { get; set; }

        public string OfferPageColorText3 { get; set; }

        public string OfferPageSpan3 { get; set; }

        public string OfferPageColorSpan3 { get; set; }

        public string OfferPageTextType3 { get; set; }

        public string OfferPageFillColor { get; set; }

        public virtual OfferType OfferType{ get; set; }

        [NotMapped]
        public List<OfferType> OfferTypes{ get; set; }

        public string OfferImage { get; set; }

        public string MixText()
        {
            var t1 = string.IsNullOrEmpty(OfferPageText1) ? "<br>" :
                $"<span class=\"caption-{OfferPageTextType1}\" style=\"color: {OfferPageColorText1}\">{OfferPageText1}</span>";
            if (!string.IsNullOrEmpty(OfferPageSpan1))
                t1 = t1.Replace(OfferPageSpan1, $"<span style=\"color: {OfferPageColorSpan1}\">{OfferPageSpan1}</span>");

            var t2 = string.IsNullOrEmpty(OfferPageText2) ? "<br>" :
                $"<span class=\"caption-{OfferPageTextType2}\" style=\"color: {OfferPageColorText2}\">{OfferPageText2}</span>";
            if (!string.IsNullOrEmpty(OfferPageSpan2))
                t2 = t2.Replace(OfferPageSpan2, $"<span style=\"color: {OfferPageColorSpan2}\">{OfferPageSpan2}</span>");

            var t3 = string.IsNullOrEmpty(OfferPageText3) ? "<br>" :
                $"<span class=\"caption-{OfferPageTextType3}\" style=\"color: {OfferPageColorText3}\">{OfferPageText3}</span>";
            if (!string.IsNullOrEmpty(OfferPageSpan3))
                t3 = t3.Replace(OfferPageSpan3, $"<span style=\"color: {OfferPageColorSpan3}\">{OfferPageSpan3}</span>");

            return $"{t1}\n{t2}\n{t3}";
        }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Ofertas
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class OfferPagePreview : IPagePreview
    {
        public string OfferPageName { get; set; }
        public string ImagenProductosOfertas { get; set; }
        public string TextoOfertas1 { get; set; }
        public string TextoOfertas2 { get; set; }
        public string TextoOfertas3 { get; set; }

        public string ColorTextoOfertas1 { get; set; }
        public string ColorTextoOfertas2 { get; set; }
        public string ColorTextoOfertas3 { get; set; }

        public string TipoTexto1 { get; set; }
        public string TipoTexto2 { get; set; }
        public string TipoTexto3 { get; set; }

        public string TextoResaltadoOfertas1 { get; set; }
        public string TextoResaltadoOfertas2 { get; set; }
        public string TextoResaltadoOfertas3 { get; set; }

        public string ColorTextoResaltadoOfertas1 { get; set; }
        public string ColorTextoResaltadoOfertas2 { get; set; }
        public string ColorTextoResaltadoOfertas3 { get; set; }

        public string TipoOferta { get; set; }
        public string ColorFondoOfertas { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}