using BenavidesFarm.DataModels.Models.Pages.Elements.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    public class InterestRegion
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public string Name { get; set; }

        [Display(Name = "Estado")]
        public bool Active { get; set; }

        [Display(Name = "Fecha")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Direcciones electrónicas")]
        [Required(ErrorMessage = "Debe especificar al menos una dirección electrónica")]
        [MailCollection(errorMessagePlaceHolder: "La dirección electrónica {0} no es correcta")]
        public string MailCollection { get; set; }
    }
}
