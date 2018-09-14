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
    /// Clase que representa el modelo para la página de Sucursales
    /// </summary>
    public class BranchPage
    {
        public int BranchPageId { get; set; }

        [Required]
        public string BranchPageTitle { get; set; }

        public string BranchPageMessage { get; set; }

        public string BranchPageBranchNames { get; set; }

        [Required]
        [DisplayName("Estado")]
        public bool BranchPageActive { get; set; }

        [Required]
        [DisplayName("Fecha de Creación")]
        public DateTime BranchPageCreatedDate { get; set; }

        [Required]
        [DisplayName("Id")]
        public string BranchPageCustomValue { get; set; }

        public string BranchPageColorMessage { get; set; }

        public string BranchPageColorTextBranchNames { get; set; }


        [NotMapped]
        public IList<ImageSection> HeadImages { get; set; }

        [NotMapped]
        public IList<Branch> Branchs { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Sucursales
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class BranchPagePreview : IPagePreview
    {
        public string BranchPageName { get; set; }

        public List<HeadImage> Encabezado { get; set; }

        public string TextoSucursales1 { get; set; }
        public string TextoSucursales2 { get; set; }
        public string ColorTextoSucursales1 { get; set; }
        public string ColorTextoSucursales2 { get; set; }
    }
}