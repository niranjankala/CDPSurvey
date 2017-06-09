using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CDPReporting.UI.Startup))]
namespace CDPReporting.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
