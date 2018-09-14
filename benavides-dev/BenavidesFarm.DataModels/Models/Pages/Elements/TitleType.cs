namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Título para los encabezados de ofertas y blogs
    /// </summary>
    public class TitleType
    { 
        public int TitleTypeId { get; set; }

        public string TitleTypeMessage { get; set; }

        public string TitleTypeColor { get; set; }

        public string TitleTypeHoverColor { get; set; }

        public string TitleTypeBgColor { get; set; }

        public string TitleTypeSpan { get; set; }

        public string TitleTypeCustomValue { get; set; }
    }
}