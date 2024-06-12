using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Maio11_Best.Startup))]
namespace Maio11_Best
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
