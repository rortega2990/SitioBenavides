using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Únete al Equipo
    /// </summary>
    public class JoinTeamPage
    {
        public int JoinTeamPageId { get; set; }

        [Required]
        public string JoinTeamPageTitle { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool JoinTeamPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime JoinTeamPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string JoinTeamPageCustomValue { get; set; }

        public string JoinTeamPageSubText1 { get; set; }

        public string JoinTeamPageSubText2 { get; set; }

        public string JoinTeamPageColorText1 { get; set; }

        public string JoinTeamPageColorText2 { get; set; }

        [NotMapped]
        public List<InterestArea> InterestAreas { get; set; }

        [NotMapped]
        public IList<ImageSection> HeadImages { get; set; }

        [NotMapped]
        public List<InterestRegion> InterestRegions { get; set; }


    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Únete al Equipo
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class JoinTeamPagePreview : IPagePreview
    {
        public string JoinTeamPageName { get; set; }

        public string ColorText1JoinTeamPage { get; set; }
        public string ColorText2JoinTeamPage { get; set; }
        public string SubTextJoinTeamPage1 { get; set; }
        public string SubTextJoinTeamPage2 { get; set; }

        public List<HeadImage> Encabezado { get; set; }
    }
}
