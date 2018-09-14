using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Sections;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Tarjetas
    /// </summary>
    public class BillingPage
    {
        public int BillingPageId { get; set; }

        [Required]
        public string BillingPageTitle { get; set; }

        public int BenefitSectionId { get; set; }

        public int IncrementBenefitSectionId { get; set; }

        public int LabSectionId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool BillingPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime BillingPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string BillingPageCustomValue { get; set; }


        [NotMapped]
        public IList<ImageSection> HeadImages { get; set; }

        public virtual BenefitSection BenefitSection { get; set; }

        public virtual IncrementBenefitSection IncrementBenefitSection { get; set; }

        public virtual LabSection LabSection { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Tarjetas
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class BillingPagePreview : IPagePreview
    {
        public string BillingPageName { get; set; }
        public string AumentaBeneficiosTarjeta { get; set; }
        public string BeneficiosTarjeta { get; set; }
        public string BeneficiosTarjetaParrafo { get; set; }
        public string TituloLaboratorios { get; set; }

        public List<HeadImage> ImagesLab { get; set; }
        public List<HeadImage> Encabezado { get; set; }
        public string BeneficiosTarjetaImagen { get; set; }
        public string BeneficiosTarjetaImagenXs { get; set; }
        public string AumentaBeneficiosImagen1 { get; set; }
        public string AumentaBeneficiosImagen2 { get; set; }
    }
}