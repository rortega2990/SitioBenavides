using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    public class Estados
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Estado")]
        public string Name { get; set; }

        [Display(Name = "Longitud")]
        public string Lng { get; set; }

        [Display(Name = "Latitud")]
        public string Lat { get; set; }

    }
}
