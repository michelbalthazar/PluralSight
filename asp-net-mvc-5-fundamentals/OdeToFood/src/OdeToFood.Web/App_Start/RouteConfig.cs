using System.Web.Mvc;
using System.Web.Routing;

namespace OdeToFood.Web
{
    public class RouteConfig
    {
        // Add Route Handler
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // using RouteCollectionExtensions 
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // Using route
            Route myRoute = new Route("{controller}/{action}/{id}",
                new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" }, { "id", "1" } },
                new MvcRouteHandler());
            routes.Add(myRoute);
        }
    }
}