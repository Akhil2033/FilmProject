using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FilmProject.Startup))]
namespace FilmProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
