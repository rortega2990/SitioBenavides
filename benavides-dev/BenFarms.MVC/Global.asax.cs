using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BenavidesFarm.DataModels.Models;
using Newtonsoft.Json;
using BenavidesFarm.DataModels.Migrations;
using BenFarms.MVC.Controllers;

namespace BenFarms.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Método donde se inicia la configuración de todo el sitio
        /// Las rutas, los filtros, las areas como el admin, los formatos de respuesta, etc.
        /// </summary>
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                //BundleConfig.RegisterBundles(BundleTable.Bundles);

                //Define Formatters
                var formatters = GlobalConfiguration.Configuration.Formatters;
                var jsonFormatter = formatters.JsonFormatter;
                var settings = jsonFormatter.SerializerSettings;
                settings.Formatting = Formatting.Indented;
                //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var appXmlType = formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
                formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

                //Add CORS Handler
                GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
                GlobalConfiguration.Configuration.EnsureInitialized();

                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyApplicationDbContext, Configuration>(true));
                new MyApplicationDbContext().Database.Initialize(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Database", ex);
            }
        }

        /// <summary>
        /// Método que captura todos los errores que ocurren en el sitio.
        /// Cuando ocurre un error todo pasa por aquí y se le da un tratamiento a la excepción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError();
            //LogManager.GetCurrentClassLogger().Log(LogLevel.Error, ex);

            //if (code != 404)
            //{
            //    var mail = new MailMessage { From = new MailAddress("automated@contoso.com") };
            //    mail.To.Add(new MailAddress("administrator@contoso.com"));
            //    mail.Subject = "Site Error at " + DateTime.Now;
            //    mail.Body = "Error Description: " + error.Message;
            //    var server = new SmtpClient { Host = "your.smtp.server" };
            //    server.Send(mail);
            //}

            // Transfer the user to the appropriate custom error page
            Exception lastErrorWrapper = Server.GetLastError();

            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            //routeData.Values.Add("exception", lastErrorWrapper);

            if (lastErrorWrapper.GetType() == typeof(HttpException))
            {
                var code = ((HttpException) lastErrorWrapper).GetHttpCode();
                routeData.Values.Add("action", code == 404 ? "NotFound" : "Index");
                //routeData.Values.Add("statusCode", code);
            }
            else
            {
                routeData.Values.Add("action", "Index");
                //routeData.Values.Add("statusCode", 500);
            }

            Response.TrySkipIisCustomErrors = true;
            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            Response.End();
        }
    }
}
