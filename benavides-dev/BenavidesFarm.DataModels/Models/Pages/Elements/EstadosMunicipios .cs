using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    public class EstadosMunicipios
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "EstadoId")]
        public Estados Estado { get; set; }

        [Display(Name = "MunicipioId")]
        public Municipios Municipio { get; set; }
    }
}
