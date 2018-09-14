using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using System.ComponentModel;
using System.Collections.Generic;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Pilares
    /// </summary>
    public class PillarPage
    {
        public int PillarPageId { get; set; }

        [Required]
        public string PillarPageTitle { get; set; }

        [Required]
        [DisplayName("Estado")]
        public bool PillarPageActive { get; set; }

        [Required]
        [DisplayName("Fecha de Creación")]
        public DateTime PillarPageCreatedDate { get; set; }

        [Required]
        [DisplayName("Id")]
        public string PillarPageCustomValue { get; set; }

        public string PillarPageColorText1 { get; set; }

        public string PillarPageText1 { get; set; }

        public string PillarPageColorText2 { get; set; }

        public string PillarPageText2 { get; set; }

        public string PillarPageImage { get; set; }

        [NotMapped]
        public IList<Pillar> Pillars { get; set; }

        [NotMapped]
        public IList<Quote> Quotes { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Pilares
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class PillarPagePreview : IPagePreview
    {
        public string PillarPageName { get; set; }

        public List<HeadImage> Encabezado { get; set; }
        public string TextoPilares1 { get; set; }
        public string TextoPilares2 { get; set; }
        public string ColorTextoPilares1 { get; set; }
        public string ColorTextoPilares2 { get; set; }
        public string ImagenPaginaPilares { get; set; }

    }
}