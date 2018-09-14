using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Número de Sad
    /// </summary>
    public class SaDTypeNumber
    {
        public int SaDTypeNumberId { get; set; }

        [Display(Name ="Ciudad")]
        public string SaDTypeNumberCity { get; set; }

        [Display(Name = "Teléfono")]
        public string SaDTypeNumberPhone { get; set; }

        [Display(Name = "Estado")]
        public bool SaDTypeNumberActive { get; set; }
    }
}