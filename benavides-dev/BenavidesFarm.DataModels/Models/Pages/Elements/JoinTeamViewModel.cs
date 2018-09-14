using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo vista de �nete al equipo de un usuario
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
        [Display(Name = "*Tel�fono:")]
        public string TeamPhoneNumber { get; set; }

        [Required]
        [Display(Name = "*E-mail:")]
        public string TeamEmail { get; set; }

        [Display(Name = "�rea de inter�s:")]
        public string TeamInterestArea { get; set; }

        [Display(Name = "Direcci�n:")]
        public string TeamAddress { get; set; }
        [Display(Name = "Regi�n de inter�s:")]
        public string InterestRegion { get; set; }
    }
}