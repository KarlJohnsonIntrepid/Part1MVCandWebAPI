using System.Web.Mvc;
using System.Web.Routing;

namespace Orchard.Candidate.Net
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                //Explicitly added namespace to prevent ambiguous reference
                namespaces: new[] { "Orchard.Candidate.Net.Controllers" }
            );
        }
    }
}
