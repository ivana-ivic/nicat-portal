using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NiCATPortal.Startup))]
namespace NiCATPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
