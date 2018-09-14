using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Ofertas en la página de Inicio
    /// </summary>
    public class OfferSection
    {
        public int OfferSectionId { get; set; }

        public string OfferSectionTitle { get; set; }

        public string OfferSectionColorTitle { get; set; }

        [Required]
        public string OfferSectionCustomValue { get; set; }


        public virtual IList<Product> Products { get; set; }

        public IList<HomePage> HomePages { get; set; }
    }
}