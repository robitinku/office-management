using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Office_Management_System.Startup))]
namespace Office_Management_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
