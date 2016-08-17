using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LandmarksBlog.Startup))]
namespace LandmarksBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
