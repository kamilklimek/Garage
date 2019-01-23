using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClassicGarageAuth.Startup))]
namespace ClassicGarageAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
