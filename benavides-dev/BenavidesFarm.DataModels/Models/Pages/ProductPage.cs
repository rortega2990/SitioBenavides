using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Productos de Promociones de Fose
    /// </summary>
    public class ProductPage
    {
        public int ProductPageId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string ProductPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime ProductPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string ProductPageCustomValue { get; set; }

        public int PromocionPageId { get; set; }

        public int ProductId { get; set; }

        public string ProductPageBgColor { get; set; }

        public string ProductPageTextTitle { get; set; }

        public string ProductPageColorTextTitle { get; set; }

        public string ProductPageTextDescription1 { get; set; }

        public string ProductPageTextDescription2 { get; set; }

        public string ProductPageTextCharacteristic1 { get; set; }

        public string ProductPageTextCharacteristic2 { get; set; }

        public string ProductPageColorTextDescription1 { get; set; }

        public string ProductPageColorTextDescription2 { get; set; }

        public string ProductPageColorTextCharacteristic1 { get; set; }

        public string ProductPageColorTextCharacteristic2 { get; set; }

        public PromocionPage PromocionPage { get; set; }

        public virtual Product Product { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Productos de Promociones de Fose
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class ProductPagePreview : IPagePreview
    {
        public string ProductPageName { get; set; }
        public string BgColorProductPage { get; set; }

        public string TextTitleProductPage { get; set; }

        public string ColorTextTitleProductPage { get; set; }

        public string TextDescription1ProductPage { get; set; }

        public string TextDescription2ProductPage { get; set; }

        public string TextCharacteristic1ProductPage { get; set; }

        public string TextCharacteristic2ProductPage { get; set; }

        public string ColorTextDescription1ProductPage { get; set; }

        public string ColorTextDescription2ProductPage { get; set; }

        public string ColorTextCharacteristic1ProductPage { get; set; }

        public string ColorTextCharacteristic2ProductPage { get; set; }

        public string ImageProductPage { get; set; }
        public int? ProductPageId { get; set; }

        public int? PromocionPageId { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}