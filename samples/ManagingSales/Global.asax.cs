using Autofac;
using Autofac.Integration.Mvc;
using GogoKit;
using GogoKit.Enumerations;
using GogoKit.Services;
using ManagingSales.Services;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ManagingSales
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string ClientId = "atE6gCHFmBicKQSy3JLq";
        private const string ClientSecret = "kECcQ3Sz68q40fobgDRrjZ7lBniVMIJNAwTGtxyuOUWmYevLsXdhHa5K291F";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterIOC();
        }

        private void RegisterIOC()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();

            builder.Register(c => new ViagogoClient(ClientId,
                                                    ClientSecret,
                                                    new ProductHeaderValue("GogoKit-Samples"),
                                                    new GogoKitConfiguration
                                                    {
                                                        ViagogoApiEnvironment = ApiEnvironment.Sandbox,
                                                        CaptureSynchronizationContext = true
                                                    },
                                                    c.Resolve<IOAuth2TokenStore>()))
                   .As<IViagogoClient>()
                   .InstancePerRequest();
            builder.Register(c => new CookieOAuth2TokenStore()).As<IOAuth2TokenStore>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
