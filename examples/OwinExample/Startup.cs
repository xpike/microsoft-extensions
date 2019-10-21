using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using OwinExample.Services;
using System;
using System.Web.Http;
using XPike.Extensions.DependencyInjection;
using XPike.Extensions.DependencyInjection.Owin;
using XPike.Extensions.DependencyInjection.WebApi;

[assembly: OwinStartup(typeof(OwinExample.Startup))]

namespace OwinExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure WebAPI the way you normally would
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // Register your services and tell Owin to use Microsoft Dependency Injection
            app.UseMicrosoftDependencyInjection(ConfigureServices(), config);

            // Register other middlerware...
            // app.Use(...)
            // ...

            // Tell Owin to use WebAPI the way you normally would.
            app.UseWebApi(config);
        }

        private IServiceProvider ConfigureServices()
        {
            // Create a new Microsoft Dependency Injection ServiceCollection
            IServiceCollection services = new ServiceCollection();
            
            // Add Verifification support
            services.AddServiceProviderVerification();

            // Add all WebAPI controllers in the solution
            services.AddApiControllers();

            // Register your services
            services.AddSingleton<IPersonService, PersonService>();

            // Build and verify the ServiceProvider
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
            return provider;
        }
    }
}
