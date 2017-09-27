using Microsoft.Owin;
using Orchard.Candidate.Net;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Orchard.Candidate.Net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
