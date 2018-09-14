using System;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Contacto de usuario
    /// </summary>
    public class ContactUser
    {
        public int ContactUserId { get; set; }

        [Required]
        [Display(Name = "Nombre (s)")]
        public string Names { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Sugerencias")]
        public string Suggests { get; set; }

        [Required]
        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreation { get; set; }
    }
}