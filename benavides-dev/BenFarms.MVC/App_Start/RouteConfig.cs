using System.Web.Mvc;
using System.Web.Routing;

namespace BenFarms.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                 name: "Sucursales",
                 url: "Sucursales",
                 defaults: new
                 {
                     controller = "Branch",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                name: "SucursalesSearch",
                url: "Sucursales/Search",
                namespaces: new string[] { "BenFarms.MVC.Controllers" },
                defaults: new { controller = "Branch", action = "Search", searchCriteria = UrlParameter.Optional }
            );

            //Folleto
            routes.MapRoute(
            name: "Folleto",
            url: "Folleto",
            defaults: new
            {
                controller = "Promotions",
                action = "Descarga"
            },
            namespaces: new[] { "BenFarms.MVC.Controllers" }
        );

            //PaginaPuente
            routes.MapRoute(
            name: "PaginaPuente",
            url: "PaginaPuente",
            defaults: new
            {
                controller = "Billing",
                action = "PaginaPuente"
            },
            namespaces: new[] { "BenFarms.MVC.Controllers" }
        );


            //SADCupones
            //SADCuponesCDMX
            //Promotions/SADCuponesView
            routes.MapRoute(
    name: "SADCuponesCDMX",
    url: "SADCuponesCDMX",
    defaults: new
    {
        controller = "Promotions",
        action = "SADCuponesView"
    },
    namespaces: new[] { "BenFarms.MVC.Controllers" }
);



            //SADCupones
            //SADCuponesCDMX
            //Promotions/SADCuponesView
            routes.MapRoute(
    name: "DinamicaHuggies",
    url: "DinamicaHuggies",
    defaults: new
    {
        controller = "Promotions",
        action = "DinamicaHuggies"
    },
    namespaces: new[] { "BenFarms.MVC.Controllers" }
);


            routes.MapRoute(
name: "BuzonSugerenciasSistemas",
url: "BuzonSugerenciasSistemas",
defaults: new
{
controller = "Promotions",
action = "BuzonSugerencias"
},
namespaces: new[] { "BenFarms.MVC.Controllers" }
);



            //landingno7Serum
            routes.MapRoute(
name: "no7Serum",
url: "no7Serum",
defaults: new
{
    controller = "Promotions",
    action = "landingno7Serum"
},
namespaces: new[] { "BenFarms.MVC.Controllers" }
);



            routes.MapRoute(
name: "no7",
url: "no7",
defaults: new
{
    controller = "Promotions",
    action = "landingno7"
},
namespaces: new[] { "BenFarms.MVC.Controllers" }
);




            routes.MapRoute(
                 name: "TarjetaBenavides",
                 url: "TarjetaBenavides",
                 defaults: new
                 {
                     controller = "Billing",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );
     
            routes.MapRoute(
         name: "bd",
         url: "bd",
         defaults: new
         {
             controller = "Promotions",
             action = "BirthdayView"
         },
         namespaces: new[] { "BenFarms.MVC.Controllers" }
     );



            routes.MapRoute(
              name: "VerPromociones",
              url: "VerPromociones",
              defaults: new
              {
                  controller = "Billing",
                  action = "VerPromociones"
              },
              namespaces: new[] { "BenFarms.MVC.Controllers" }
          );



            routes.MapRoute(
                 name: "Consultorios",
                 url: "Consultorios",
                 defaults: new
                 {
                     controller = "DoctorsOffice",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Beauty",
                 url: "Beauty",
                 defaults: new
                 {
                     controller = "Fose",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "BeautyProducts",
                 url: "Beauty/Products/{id}",
                 defaults: new
                 {
                     controller = "Fose",
                     action = "Products",
                     id = UrlParameter.Optional
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "BeautyProductsImage",
                 url: "Beauty/ProductsImage/{id}",
                 defaults: new
                 {
                     controller = "Fose",
                     action = "ImageProduct",
                     id = UrlParameter.Optional
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );


            routes.MapRoute(
                name: "BlogDesc",
                url: "Blog/BlogDesc/{id}",
                defaults: new { controller = "Blog", action = "BlogDesc" },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "BlogNews",
                url: "Blog/News/{id}",
                defaults: new { controller = "Blog", action = "News" },
                constraints: new { id = @"\d+" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "users", action = "Index", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
                 name: "Blog",
                 url: "Blog/{type}",
                 defaults: new
                 {
                     controller = "Blog",
                     action = "Blog",
                     type = UrlParameter.Optional
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "QuienesSomos",
                 url: "QuiénesSomos",
                 defaults: new
                 {
                     controller = "WhoWeAre",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "ResponsabilidadSocial",
                 url: "RSC",
                 defaults: new
                 {
                     controller = "Pillar",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Servicios",
                 url: "Servicios",
                 defaults: new
                 {
                     controller = "Service",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Inversionistas",
                 url: "Inversionistas",
                 defaults: new
                 {
                     controller = "Investor",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Proveedores",
                 url: "Proveedores",
                 defaults: new
                 {
                     controller = "Provider",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Unete",
                 url: "Únete",
                 defaults: new
                 {
                     controller = "JoinTeam",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Privacidad",
                 url: "Privacidad",
                 defaults: new
                 {
                     controller = "Privacity",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );

            routes.MapRoute(
                 name: "Aniversario",
                 url: "Aniversario",
                 defaults: new
                 {
                     controller = "OneHundredYears",
                     action = "Index"
                 },
                 namespaces: new[] { "BenFarms.MVC.Controllers" }
             );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                namespaces: new string[] { "BenFarms.MVC.Controllers" },
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CatchAll",
                url: "{*any}",
                defaults: new { controller = "Error", action = "NotFound" }
                );

            
        }
    }
}
