using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JhulayLal_Enterprise.Startup))]
namespace JhulayLal_Enterprise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
