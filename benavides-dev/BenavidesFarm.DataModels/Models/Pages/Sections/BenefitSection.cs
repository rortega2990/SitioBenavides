using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Beneficios en la página de Tarjetas
    /// </summary>
    public class BenefitSection
    {
        public int BenefitSectionId { get; set; }

        public string BenefitSectionTitle { get; set; }

        public string BenefitSectionDiv { get; set; }

        public string BenefitSectionParagraph { get; set; }

        public string BenefitSectionImage { get; set; }

        public string BenefitSectionImageXS { get; set; }

        [Required]
        public string BenefitSectionCustomValue { get; set; }

        public List<BillingPage> BillingPages { get; set; }
    }
}