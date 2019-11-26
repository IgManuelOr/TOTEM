using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Token.Startup))]
namespace Token
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
