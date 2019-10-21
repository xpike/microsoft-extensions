using Owin;
using System;
using System.Web.Http;
using XPike.Extensions.DependencyInjection.WebApi;

namespace XPike.Extensions.DependencyInjection.Owin
{
    /// <summary>
    /// Extension methods for IAppBuilder to suppport the use of Microsoft.Extensions.DependencyInjection in Owin
    /// applications.
    /// </summary>
    public static class IAppBuilderExtensions
    {
        /// <summary>
        /// Configures and uses Microsoft dependency injection for this application.
        /// This should be first in the pipeline.
        /// </summary>
        /// <param name="app">This IAppBuilder instance.</param>
        /// <param name="provider">The IServiceProvider instance.</param>
        /// <param name="configuration">The HttpConfiguration instance for this application.</param>
        /// <returns>IAppBuilder</returns>
        /// <exception cref="System.ArgumentNullException">app</exception>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        /// <example>
        /// ```cs
        /// public class Startup
        /// {
        ///     public void Configuration(IAppBuilder app)
        ///     {
        ///         HttpConfiguration config = new HttpConfiguration();
        ///         config.MapHttpAttributeRoutes();
        ///         
        ///         IServiceProvider provider = ConfigureServices();
        ///         
        ///         app.UseMicrosoftDependencyInjection(provider, config);
        ///         
        ///         app.UseWebApi(config);
        ///     }
        ///     
        ///     private IServiceProvider ConfigureServices()
        ///     {
        ///         IDependencyCollection services = new ServicesCollection();
        ///         
        ///         // register services
        ///         services.AddSingleton&lt;ILogger, Logger&gt;();
        ///         
        ///         // Add the WebApi controllers to the collection
        ///         services.AddApiControllers();
        ///         
        ///         return services.BuildServiceProvider();
        ///     }
        /// }
        /// ```
        /// </example>
        public static IAppBuilder UseMicrosoftDependencyInjection(this IAppBuilder app, IServiceProvider provider, HttpConfiguration configuration)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.DependencyResolver = new MicrosoftDependencyResolver(provider);
            app.Use<DependencyInjectionMiddleware>(provider);

            return app;
        }
    }
}
