using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Documento
    /// </summary>
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }

        [Display(Name = "Nombre del Tipo de Documento")]
        public string DocumentName { get; set; }

        [Display(Name = "Descripción del Tipo de Documento")]
        public string DocumentDescription { get; set; }

        [Display(Name = "Estado")]
        public bool DocumentActive { get; set; }

        public virtual List<DocumentFiles> ProviderFiles { get; set; }
    }
}