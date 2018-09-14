using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Fose en la página de Inicio
    /// </summary>
    public class FoseSection
    {
        public int FoseSectionId { get; set; }

        [Required]
        public string FoseSectionTitle { get; set; }

        [StringLength(10)]
        public string FoseSectionColorTitle { get; set; }

        public string FoseSectionWord1 { get; set; }

        [StringLength(10)]
        public string FoseSectionColorWord1 { get; set; }

        public string FoseSectionWord2 { get; set; }

        [StringLength(10)]
        public string FoseSectionColorWord2 { get; set; }

        [StringLength(1024)]
        public string FoseSectionImage { get; set; }

        [StringLength(1024)]
        public string FoseSectionImageLogo { get; set; }

        [Required]
        public string FoseSectionCustomValue { get; set; }


        public List<HomePage> HomePages { get; set; }
    }
}