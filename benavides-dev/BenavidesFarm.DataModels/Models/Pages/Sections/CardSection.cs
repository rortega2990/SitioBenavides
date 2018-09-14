using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Tarjetas en la página de Inicio
    /// </summary>
    public class CardSection
    {
        public int CardSectionId { get; set; }

        [Required]
        public string CardSectionTitle { get; set; }

        public string CardSectionColorTitle { get; set; }

        public string CardSectionUrl { get; set; }

        public string CardSectionImage { get; set; }

        public string CardSectionImageXS { get; set; }

        [Required]
        public string CardSectionCustomValue { get; set; }


        public List<HomePage> HomePages { get; set; }
    }
}