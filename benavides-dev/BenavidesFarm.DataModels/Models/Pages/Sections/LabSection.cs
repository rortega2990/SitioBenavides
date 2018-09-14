using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Laboratorios en la página de Tarjetas
    /// </summary>
    public class LabSection
    {
        public int LabSectionId { get; set; }

        [Required]
        public string LabSectionTitle { get; set; }

        [Required]
        public string LabSectionCustomValue { get; set; }


        [NotMapped]
        public IList<ImageSection> ImageSections { get; set; }

        [NotMapped]
        public List<BillingPage> BillingPages { get; set; }
    }
}