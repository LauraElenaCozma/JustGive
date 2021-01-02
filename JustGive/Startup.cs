using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JustGive.Startup))]
namespace JustGive
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
