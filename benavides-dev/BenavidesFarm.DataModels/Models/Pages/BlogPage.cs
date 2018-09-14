using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Blogs
    /// </summary>
    public class BlogPage
    {
        public int BlogPageId { get; set; }

        [Required]
        public string BlogPageTitle { get; set; }

        public int BlogTypeId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        [StringLength(15)]
        public string BlogPageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime BlogPageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string BlogPageCustomValue { get; set; }

        public string BlogPageColorBgHead { get; set; }

        public string BlogPageColorTextHead { get; set; }

        public string BlogPageTextHead { get; set; }

        public string BlogPageColorTextDescHead { get; set; }

        public string BlogPageTextDesc { get; set; }

        public string BlogPageTitleDesc { get; set; }

        public string BlogPageColorTitleDesc { get; set; }

        public string BlogPageColorBgTitleDesc { get; set; }

        public string BlogPageImage { get; set; }

        public virtual BlogType BlogType{ get; set; }

        [NotMapped]
        public IList<BlogType> BlogTypes{ get; set; }

        public virtual IList<NewsPage> News { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Blogs
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class BlogPagePreview : IPagePreview
    {
        public string BlogPageName { get; set; }

        public List<HeadImage> Encabezado { get; set; }

        public string ColorBgHeadBlogPage { get; set; }

        public string ColorTextHeadBlogPage { get; set; }

        public string TextHeadBlogPage { get; set; }

        public string ColorTextDescHeadBlogPage { get; set; }

        public string TextDescBlogPage { get; set; }

        public string TitleDescBlogPage { get; set; }

        public string ColorTitleDescBlogPage { get; set; }

        public string ColorBgTitleDescBlogPage { get; set; }

        public string ImageBlogPage { get; set; }

        public string TipoBlog { get; set; }
    }
}