using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Sections;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Fose
    /// </summary>
    public class FosePage
    {
        public int FosePageId { get; set; }

        public string FosePageTitle { get; set; }

        public string FoseTextBranch { get; set; }

        [Required]
        [Display(Name = "Estado")]
        [StringLength(15)]
        public string FosePageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime FosePageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string FosePageCustomValue { get; set; }


        [NotMapped]
        public IList<ImageSection> HeadImages { get; set; }

        public virtual IList<PromocionPage> Promocions { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Fose
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class FosePagePreview : IPagePreview
    {
        public List<HeadImage> Encabezado { get; set; }

        public string FosePageName { get; set; }

        public List<HeadImage> Promocions { get; set; }

        public string TextoSucursalesFose { get; set; }
    }
}
