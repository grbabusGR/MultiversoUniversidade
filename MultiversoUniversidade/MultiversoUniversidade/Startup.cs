using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiversoUniversidade.Startup))]
namespace MultiversoUniversidade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
