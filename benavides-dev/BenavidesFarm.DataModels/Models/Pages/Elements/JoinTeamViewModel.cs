using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo vista de Únete al equipo de un usuario
    /// </summary>
    public class JoinTeamViewModel
    {
        [Required]
        [Display(Name = "*Nombre (s):")]
        public string TeamNames { get; set; }

        [Required]
        [Display(Name = "*Apellidos:")]
        public string TeamLastnames { get; set; }

        [Required]
        [Display(Name = "*Teléfono:")]
        public string TeamPhoneNumber { get; set; }

        [Required]
        [Display(Name = "*E-mail:")]
        public string TeamEmail { get; set; }

        [Display(Name = "Área de interés:")]
        public string TeamInterestArea { get; set; }

        [Display(Name = "Dirección:")]
        public string TeamAddress { get; set; }
        [Display(Name = "Región de interés:")]
        public string InterestRegion { get; set; }
    }
}