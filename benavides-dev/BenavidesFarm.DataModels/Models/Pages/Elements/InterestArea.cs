using BenavidesFarm.DataModels.Models.Pages.Elements.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Área de interés
    /// </summary>
    public class InterestArea
    {
        [Display(Name = "Id")]
        public int InterestAreaId { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public string InterestAreaName { get; set; }

        [Display(Name = "Estado")]
        public bool InterestAreaActive { get; set; }

        [Display(Name = "Fecha")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Direcciones electrónicas")]
        [Required(ErrorMessage = "Debe especificar al menos una dirección electrónica")]
        [MailCollection(errorMessagePlaceHolder:"La dirección electrónica {0} no es correcta")]
        public string MailCollection { get; set; }
    }
}
