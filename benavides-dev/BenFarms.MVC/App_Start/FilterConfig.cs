using System.Web;
using System.Web.Mvc;
using BenFarms.MVC.ActionFilters;

namespace BenFarms.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleExceptionAttribute());
        }
    }
}
