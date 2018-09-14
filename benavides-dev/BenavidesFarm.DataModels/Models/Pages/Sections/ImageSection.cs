using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    /// <summary>
    /// Clase para guardar los datos de aquellas páginas que contienen encabezado tipo carrusel
    /// Se guarda la imagen, el texto y el color del texto, y el nombre de la página al cual pertenece
    /// </summary>
    public class ImageSection
    {
        public int ImageSectionId { get; set; }

        [StringLength(1024)]
        public string ImageSectionImage { get; set; }

        [Required]
        [Index]
        public int ImageSectionPageId { get; set; }

        [Required]
        [Index]
        [StringLength(100)]
        public string ImageSectionPageName { get; set; }

        [Required]
        public bool ImageSectionActive { get; set; }

        public string ImageSectionText { get; set; }
        public string ImageSectionColorText { get; set; }

        public string Link { get; set; }
    }
}