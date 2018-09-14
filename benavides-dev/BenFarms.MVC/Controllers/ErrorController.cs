using System.Web.Mvc;

namespace BenFarms.MVC.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        // GET: Error/Error
        public ActionResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult Database()
        {
            Response.StatusCode = 423;
            return View();
        }
    }
}