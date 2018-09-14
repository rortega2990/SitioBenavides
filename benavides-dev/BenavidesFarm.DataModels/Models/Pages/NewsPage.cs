using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Noticias de Blogs
    /// </summary>
    public class NewsPage
    {
        public int NewsPageId { get; set; }

        [Required]
        public string NewsPageTitle { get; set; }

        public int BlogPageId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string NewsPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime NewsPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string NewsPageCustomValue { get; set; }

        public string NewsPageColorBgHead { get; set; }

        public string NewsPageTextHead { get; set; }

        public string NewsPageColorTextHead { get; set; }

        public string NewsPageSubTextHead { get; set; }

        public string NewsPageColorSubTextHead { get; set; }

        public string NewsPageColorBgSubTextHead { get; set; }

        public string NewsPageImageHead { get; set; }

        public string NewsPageImageDescription { get; set; }

        public string NewsPageTitleDescription1 { get; set; }

        public string NewsPageTitleDescription2 { get; set; }

        public string NewsPageTextDescription1 { get; set; }

        public string NewsPageTextDescription2 { get; set; }

        public string NewsPageColorTitleDescription1 { get; set; }

        public string NewsPageColorTitleDescription2 { get; set; }

        public string NewsPageUrl { get; set; }

        public BlogPage BlogPage { get; set; }

        public int NewsPageOrder { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Noticias de Blogs
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class NewsPagePreview : IPagePreview
    {
        public string NewsPageName { get; set; }

        public int? NewsPageId { get; set; }

        public int? BlogPageId { get; set; }

        public List<HeadImage> Encabezado { get; set; }

        public string ColorBgHeadNewsPage { get; set; }

        public string TextHeadNewsPage { get; set; }

        public string ColorTextHeadNewsPage { get; set; }

        public string SubTextHeadNewsPage { get; set; }

        public string ColorSubTextHeadNewsPage { get; set; }

        public string ColorBgSubTextHeadNewsPage { get; set; }

        public string ImageHeadNewsPage { get; set; }

        public string ImageDescriptionNewsPage { get; set; }

        public string TitleDescription1NewsPage { get; set; }

        public string TitleDescription2NewsPage { get; set; }

        public string ColorTitleDescription1NewsPage { get; set; }

        public string ColorTitleDescription2NewsPage { get; set; }

        public string TextDescription1NewsPage { get; set; }
        public string TextDescription2NewsPage { get; set; }
        //public string ColorTextDescription1NewsPage { get; set; }
        //public string ColorTextDescription2NewsPage { get; set; }
    }
}