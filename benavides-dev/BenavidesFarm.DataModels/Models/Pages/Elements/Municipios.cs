using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    public class Municipios
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Municipio")]
        public string Name { get; set; }

        [Display(Name = "Longitud")]
        public string Lng { get; set; }

        [Display(Name = "Latitud")]
        public string Lat { get; set; }

    }
}
