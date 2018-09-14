using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Sections;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Producto, ya sea una oferta o un blog
    /// </summary>
    public class Product
    {
        public int ProductId{ get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        public string ProductName { get; set; }

        public int? ProductTitle_TitleTypeId { get; set; }

        public string ProductImage { get; set; }

        //[Range(1, 5)]
        public int ProductOrder { get; set; }

        public string ProductURL { get; set; }

        [Required]
        public string ProductCustomValue { get; set; }

        public string ProductSubtitle { get; set; }

        public int? BlogSectionId { get; set; }

        public int? OfferSectionId { get; set; }

        public int? ServiceTypeId { get; set; }

        public int? OfferTypeId { get; set; }

        public int? BlogTypeId { get; set; }

        public string ToStringMix()
        {
            var s = ProductTitle.TitleTypeMessage.Replace(ProductTitle.TitleTypeSpan,
                $"<span class=\"hover-color\" data-hcolor=\"{ProductTitle.TitleTypeColor}\">{ProductTitle.TitleTypeSpan}</span>");
            return s;
        }

        [ForeignKey("ProductTitle_TitleTypeId")]
        public virtual TitleType ProductTitle { get; set; }

        public virtual OfferType OfferType { get; set; }

        public virtual BlogType BlogType { get; set; }

        public BlogSection BlogSection { get; set; }

        public OfferSection OfferSection { get; set; }

        public ServiceType ServiceType { get; set; }

        public List<ProductPage> ProductPages { get; set; }

    }
}