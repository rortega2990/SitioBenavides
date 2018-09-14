using System;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Únete al equipo de usuario
    /// </summary>
    public class UserJoinedToTeam
    {
        public int UserJoinedToTeamId { get; set; }

        [Required]
        [Display(Name = "Nombre (s)")]
        public string Names { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string Lastnames { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Área de Interés")]
        public string InterestArea { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreation { get; set; }
    }
}