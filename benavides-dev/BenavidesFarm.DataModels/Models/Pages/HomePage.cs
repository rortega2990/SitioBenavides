using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Models.Pages
{
    /// <summary>
    /// Clase que representa el modelo para la página de Inicio
    /// </summary>
    public class HomePage
    {
        public int HomePageId { get; set; }

        [Required]
        public string HomePageTitle { get; set; }

        public int OfferSectionId { get; set; }       

        public int CardSectionId { get; set; }

        public int FourQuadSectionId { get; set; }

        public int DoctorsOfficeSectionId { get; set; }

        public int FoseSectionId { get; set; }        

        public int BlogSectionId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool HomePageActive { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime HomePageCreatedDate { get; set; }

        [Required]
        [Display(Name = "Id")]
        public string HomePageCustomValue { get; set; }


        [NotMapped]
        public IList<ImageSection> HeadImages { get; set; }

        [NotMapped]
        public List<OfferType> OfferTypes { get; set; }

        [NotMapped]
        public List<BlogType> BlogTypes { get; set; }

        public virtual OfferSection OfferSection { get; set; }

        public virtual CardSection CardSection { get; set; }

        public virtual FoseSection FoseSection { get; set; }

        public virtual BlogSection BlogSection { get; set; }

        public FourQuadSection FourQuadSection { get; set; }

        public DoctorsOfficeSection DoctorsOfficeSection { get; set; }
    }

    /// <summary>
    /// Clase que contiene los datos para la vista previa de la página de Inicio
    /// Los datos se serializan y se guardan en una sola tabla PagePreview, identificada con el nombre de la tabla
    /// </summary>
    [Serializable]
    public class HomePagePreview : IPagePreview
    {
        public string HomePageName { get; set; }

        public string TituloOfertas { get; set; }
        public string ColorTituloOfertas { get; set; }

        public string ImagenOfertas1 { get; set; }
        public string TextoImagenOfertas1 { get; set; }
        public string SpanImagenOfertas1 { get; set; }
        public string ColorImagenOfertas1 { get; set; }
        public string TipoOfertas1 { get; set; }

        public string ImagenOfertas2 { get; set; }
        public string TextoImagenOfertas2 { get; set; }
        public string SpanImagenOfertas2 { get; set; }
        public string ColorImagenOfertas2 { get; set; }
        public string TipoOfertas2 { get; set; }

        public string ImagenOfertas3 { get; set; }
        public string TextoImagenOfertas3 { get; set; }
        public string SpanImagenOfertas3 { get; set; }
        public string ColorImagenOfertas3 { get; set; }
        public string TipoOfertas3 { get; set; }

        public string ImagenOfertas4 { get; set; }
        public string TextoImagenOfertas4 { get; set; }
        public string SpanImagenOfertas4 { get; set; }
        public string ColorImagenOfertas4 { get; set; }
        public string TipoOfertas4 { get; set; }

        public string ImagenOfertas5 { get; set; }
        public string TextoImagenOfertas5 { get; set; }
        public string SpanImagenOfertas5 { get; set; }
        public string ColorImagenOfertas5 { get; set; }
        public string TipoOfertas5 { get; set; }

        public string TituloTarjetas { get; set; }
        public string ColorTituloTarjetas { get; set; }
        public string ImagenTarjetasWeb { get; set; }
        public string ImagenTarjetasMovil { get; set; }
        public string UrlImagenTarjetas { get; set; }

        public string TituloFose { get; set; }
        public string ColorTituloFose { get; set; }
        public string TextoFose1 { get; set; }
        public string ColorTextoFose1 { get; set; }
        public string TextoFose2 { get; set; }
        public string ColorTextoFose2 { get; set; }
        public string ImagenFoseLogo { get; set; }
        public string ImagenFoseProducto { get; set; }

        public string TituloBlog { get; set; }
        public string ColorTituloBlog { get; set; }

        public string ImagenBlog1 { get; set; }
        public string UrlImagenBlog1 { get; set; }
        public string TextoBlog1 { get; set; }
        public string ColorTextoBlog1 { get; set; }
        public string ColorOverTextoBlog1 { get; set; }
        public string ColorBgTextoBlog1 { get; set; }

        public string ImagenBlog2 { get; set; }
        public string UrlImagenBlog2 { get; set; }
        public string TextoBlog2 { get; set; }
        public string ColorTextoBlog2 { get; set; }
        public string ColorOverTextoBlog2 { get; set; }
        public string ColorBgTextoBlog2 { get; set; }

        public string ImagenBlog3 { get; set; }
        public string TextoBlog3 { get; set; }
        public string UrlImagenBlog3 { get; set; }
        public string ColorTextoBlog3 { get; set; }
        public string ColorOverTextoBlog3 { get; set; }
        public string ColorBgTextoBlog3 { get; set; }

        public string ImagenBlog4 { get; set; }
        public string TextoBlog4 { get; set; }
        public string UrlImagenBlog4 { get; set; }
        public string ColorTextoBlog4 { get; set; }
        public string ColorOverTextoBlog4 { get; set; }
        public string ColorBgTextoBlog4 { get; set; }

        public List<HeadImage> Encabezado { get; set; }
        public string TipoBlog1 { get; set; }
        public string TipoBlog2 { get; set; }
        public string TipoBlog3 { get; set; }
        public string TipoBlog4 { get; set; }

        public string QuadText1 { get; set; }
        public string QuadLink1 { get; set; }
        public string QuadImageFileName1 { get; set; }
        public string QuadBackgroundColor1 { get; set; }

        public string QuadText2 { get; set; }
        public string QuadLink2 { get; set; }
        public string QuadImageFileName2 { get; set; }
        public string QuadBackgroundColor2 { get; set; }

        public string QuadText3 { get; set; }
        public string QuadLink3 { get; set; }
        public string QuadImageFileName3 { get; set; }
        public string QuadBackgroundColor3 { get; set; }

        public string QuadText4 { get; set; }
        public string QuadLink4 { get; set; }
        public string QuadImageFileName4 { get; set; }
        public string QuadBackgroundColor4 { get; set; }

        public string DoctorsOfficeTitle { get; set; }
        public string DoctorsOfficeMessageText { get; set; }
        public string DoctorsOfficeImageFileName { get; set; }
        public string DoctorsOfficeBackgroundColor { get; set; }
        public string DoctorsOfficeLogoImageFileName { get; set; }
        public string DoctorsOfficeLink { get; set; }
        public string DoctorsOfficeTitleColor { get; set; }
        public string DoctorsOfficeMessageTextColor { get; set; }
    }

    [Serializable]
    public class HeadImage
    {
        public int HeadImageId { get; set; }
        public string HeadImageImage { get; set; }
        public int HeadImagePageId { get; set; }
        public string HeadImagePageName { get; set; }
        public bool HeadImageActive { get; set; }
        public string HeadImageText { get; set; }
        public string HeadImageColorText { get; set; }
        public string HeaderLink { get; set; }
    }
}