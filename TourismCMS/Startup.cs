using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TourismCMS.Startup))]
namespace TourismCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
