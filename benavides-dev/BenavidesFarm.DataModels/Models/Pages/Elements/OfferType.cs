using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Oferta
    /// </summary>
    public class OfferType
    {
        [Display(Name = "Id")]
        public int OfferTypeId { get; set; }

        [Display(Name = "Categoría")]
        public string OfferTypeName { get; set; }

        [Display(Name = "Estado")]
        public bool OfferTypeActive { get; set; }
    }
}
