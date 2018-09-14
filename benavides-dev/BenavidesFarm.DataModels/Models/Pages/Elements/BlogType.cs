using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    /// <summary>
    /// Clase que representa el modelo de Tipo de Blog
    /// </summary>
    public class BlogType
    {
        [Display(Name = "Id")]
        public int BlogTypeId { get; set; }

        [Display(Name = "Categoría")]
        public string BlogTypeName { get; set; }

        [Display(Name = "Estado")]
        public bool BlogTypeActive { get; set; }
    }
}
