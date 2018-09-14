using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Servicio
    /// </summary>
    public class ServiceType
    {
        public int ServiceTypeId { get; set; }

        [Display(Name = "Nombre")]
        public string ServiceTypeName { get; set; }

        [Display(Name = "Nota sobre el tipo de servicio")]
        public string ServiceTypeNameDescription { get; set; }

        [Display(Name = "Nota descriptiva sobre los servicios")]
        public string ServiceTypeProdutcsDescription { get; set; }        

        [Required]
        [Display(Name = "Estado")]
        public bool ServiceTypeActive { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}