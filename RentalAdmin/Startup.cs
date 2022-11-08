using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentalAdmin.Startup))]
namespace RentalAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
