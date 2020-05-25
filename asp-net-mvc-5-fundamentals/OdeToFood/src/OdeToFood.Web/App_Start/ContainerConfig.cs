using Autofac;
using Autofac.Integration.Mvc;
using OdeToFood.Data.Services;
using System.Web.Mvc;

namespace OdeToFood.Web
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            var build = new ContainerBuilder();

            build.RegisterControllers(typeof(MvcApplication).Assembly);
            build.RegisterType<InMemoryRestaurantData>()
                .As<IRestaurantData>()
                .SingleInstance();

            var container = build.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); 
        }
    }
}