using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Pilar
    /// </summary>
    public class Pillar
    {
        [Display(Name = "Id")]
        public int PillarId { get; set; }

        [Display(Name = "Nombre")]
        public string PillarName { get; set; }

        [Display(Name = "Estado")]
        public bool PillarActive { get; set; }

        [Display(Name = "Descripción")]
        public string PillarDescription { get; set; }

        [Display(Name = "Link")]
        public string PillarLink { get; set; }

        [Display(Name = "Imagen")]
        public string PillarImage { get; set; }

    }
}
