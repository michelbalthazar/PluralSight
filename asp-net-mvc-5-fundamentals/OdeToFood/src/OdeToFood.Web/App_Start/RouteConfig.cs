using OdeToFood.Web.Handlers;
using System.Web;
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

            // add route to use sampleHandler
            // If use SampleRouteHandler can not use MvcRouteHandler. Is one or other.
            //routes.Add(new Route("test", new SampleRouteHandler()));

            // Using route
            Route myRoute = new Route("{controller}/{action}/{id}",
                new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" }, { "id", "1" } },
                new MvcRouteHandler());
            routes.Add(myRoute);
        }
    }

    public class SampleRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SampleHandler();
        }
    }
}