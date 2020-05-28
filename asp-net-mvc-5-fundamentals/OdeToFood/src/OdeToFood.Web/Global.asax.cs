using System.Diagnostics;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(OdeToFood.Web.MvcApplication), "PreStart")]

namespace OdeToFood.Web
{
    // MVC Life cycle
    public class MvcApplication : HttpApplication
    {
        public static void PreStart()
        {
            HttpApplication.RegisterModule(typeof(LogModule));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Add MvcRouteHandler
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerConfig.RegisterContainer(GlobalConfiguration.Configuration);
        }

        protected void Application_BeginRequest()
        {
            Debug.WriteLine("Begin Request");
        }

        protected void Application_MapRequestHandler()
        {
            Debug.WriteLine("Map Handler");
        }


        protected void Application_PostMapRequestHandler()
        {
            Debug.WriteLine("Post Map Handler");
        }

        protected void Application_AcquireRequestState()
        {
            Debug.WriteLine("Request State");
        }

        protected void Application_PreRequestHandlerExecute()
        {
            Debug.WriteLine("Pre Request Handler Execute");
        }

        protected void Application_PostRequestHandlerExecute()
        {
            Debug.WriteLine("Post Request Handler Execute");
        }

        protected void Application_EndRequest()
        {
            Debug.WriteLine("End Request");
        }

        protected void Application_End()
        {
            Debug.WriteLine("End");
        }
    }
}