using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sight.Startup))]
namespace sight
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
