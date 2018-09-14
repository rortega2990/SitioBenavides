using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class QuoteInputModel
    {
        [Display(Name = "Id")]
        public int QuoteId { get; set; }

        [Display(Name = "Cita")]
        public string QuoteText { get; set; }

        [Display(Name = "Autor")]
        public bool QuoteAuthor { get; set; }

        [Display(Name = "Firma")]
        public string QuoteAuthorSign { get; set; }

        [Display(Name = "Foto del Autor")]
        public HttpFileCollectionBase QuoteAuthorPhoto { get; set; }
    }
}