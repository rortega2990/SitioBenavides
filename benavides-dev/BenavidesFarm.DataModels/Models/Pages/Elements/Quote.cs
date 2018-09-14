using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Cita
    /// </summary>
    public class Quote
    {
        [Display(Name = "Id")]
        public int QuoteId { get; set; }

        [Display(Name = "Autor")]
        public string QuoteAuthor { get; set; }

        [Display(Name = "Firma del Autor")]
        public string QuoteAuthorSign { get; set; }

        [Display(Name = "Foto del Autor")]
        public string QuoteAuthorPhoto { get; set; }

        [Display(Name = "Cita")]
        public string QuoteText { get; set; }

    }
}
