using System.Web.Http;
using BenFarms.MVC.ActionFilters;

namespace BenFarms.MVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute("APIWithAction", "api/{version}/{controller}/{action}/{id}",
                                        new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{version}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ResourceNotFound",
                routeTemplate: "api/{*uri}",
                defaults: new { controller = "Error", action = "NotFound", uri = RouteParameter.Optional });

            config.Filters.Add(new GlobalExceptionAttribute());

            //config.Services.Replace(typeof(IHttpControllerSelector), new CustomControllerSelector(config));
        }
    }

    //public class CustomControllerSelector : DefaultHttpControllerSelector
    //{
    //    public override string GetControllerName(HttpRequestMessage request)
    //    {
    //        var name = base.GetControllerName(request);
    //        if (string.IsNullOrEmpty(name))
    //        {
    //            return "Default"; //important not to include "Controller" suffix
    //        }
    //        return name;
    //    }

    //    public CustomControllerSelector(HttpConfiguration configuration)
    //        : base(configuration)
    //    {
    //    }
    //}
}
