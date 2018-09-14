using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Promociones de Fose
    /// </summary>
    public class PromocionPage
    {
        public int PromocionPageId { get; set; }

        public int PromocionPageOrder { get; set; }

        public int FosePageId { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string PromocionPageCustomValue { get; set; }

        public string PromocionPageImageLogo1 { get; set; }

        public string PromocionPageImageLogo2 { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string PromocionPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime PromocionPageCreatedDate { get; set; }

        public string PromocionPageHeadText { get; set; }

        public string PromocionPageSpanHeadText { get; set; }

        public string PromocionPageColorHeadBg { get; set; }

        public string PromocionPageHeadtextColor { get; set; }

        public string PromocionPageSpanHeadtextColor { get; set; }

        public string PromocionPageSubText1 { get; set; }

        public string PromocionPageSubText2 { get; set; }

        public FosePage FosePage { get; set; }

        public virtual List<ProductPage> ProductPages { get; set; }

        public string ToStringMix()
        {
            return PromocionPageHeadText.Replace(PromocionPageSpanHeadText,
                $"<span style=\"color:{PromocionPageSpanHeadtextColor}\">{PromocionPageSpanHeadText}</span>");
        }

        public string PromocionPageHeadImage { get; set; }

        public string PromocionPageTextFose { get; set; }

        public string PromocionPageTextColorFose { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Promociones de Fose
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class PromocionPagePreview : IPagePreview
    {
        public string PromocionPageName { get; set; }

        public int? PromocionPageId { get; set; }

        public int? FosePageId { get; set; }

        public string HeadImagePromocionPage { get; set; }

        public string TextFosePromocionPage { get; set; }

        public string TextColorFosePromocionPage { get; set; }

        public string ImageLogo1PromocionPage { get; set; }

        public string ImageLogo2PromocionPage { get; set; }

        public string HeadTextPromocionPage { get; set; }

        public string SpanHeadTextPromocionPage { get; set; }

        public string ColorHeadBgPromocionPage { get; set; }

        public string HeadtextColorPromocionPage { get; set; }

        public string SpanHeadtextColorPromocionPage { get; set; }

        public string SubText1PromocionPage { get; set; }

        public string SubText2PromocionPage { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}