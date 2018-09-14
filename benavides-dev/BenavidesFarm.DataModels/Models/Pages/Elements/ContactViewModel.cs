using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo vista de Contacto de usuario
    /// </summary>
    public class ContactViewModel
    {
        [Required]
        [Display(Name = "*Nombre (s):")]
        public string ContactNames { get; set; }

        [Required]
        [Display(Name = "*Teléfono:")]
        public string ContactPhoneNumber { get; set; }

        [Required]
        [Display(Name = "*E-mail:")]
        public string ContactEmail { get; set; }

        [Display(Name = "Sugrencias")]
        public string ContactSuggests { get; set; }
    }
}