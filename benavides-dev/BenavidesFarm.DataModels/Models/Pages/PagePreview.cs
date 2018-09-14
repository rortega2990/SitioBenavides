using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página global de vista previa
    /// Esta clase guarda los datos serializados y el nombre de la página que se va a mostrar en vista previa
    /// </summary>
    public class PagePreview
    {
        [Key]
        public string PageName { get; set; }

        public byte[] PageValue { get; set; }
    }

    /// <summary>
    /// Interfaz para las clases que representan las vistas previas
    /// </summary>
    public interface IPagePreview
    {
        List<HeadImage> Encabezado { get; set; }
    }
}