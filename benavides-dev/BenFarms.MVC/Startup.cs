using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BenFarms.MVC.Startup))]
namespace BenFarms.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
