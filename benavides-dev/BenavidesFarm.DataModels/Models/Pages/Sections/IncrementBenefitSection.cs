using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Aumenta Beneficios en la página de Tarjetas
    /// </summary>
    public class IncrementBenefitSection
    {
        public int IncrementBenefitSectionId { get; set; }

        public string IncrementBenefitSectionTitle { get; set; }

        public string IncrementBenefitSectionDiv { get; set; }

        public string IncrementBenefitSectionImage1 { get; set; }

        public string IncrementBenefitSectionImage2 { get; set; }

        [Required]
        public string IncrementBenefitSectionCustomValue { get; set; }

        public List<BillingPage> BillingPages { get; set; }
    }
}