using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase que representa el modelo para la sección de Blogs en la página de Inicio
    /// </summary>
    public class BlogSection
    {
        public int BlogSectionId { get; set; }

        public string BlogSectionTitle { get; set; }

        public string BlogSectionColorTitle { get; set; }

        [Required]
        public string BlogSectionCustomValue { get; set; }

        public virtual IList<Product> Products { get; set; }

        public IList<HomePage> HomePages { get; set; }
    }
}