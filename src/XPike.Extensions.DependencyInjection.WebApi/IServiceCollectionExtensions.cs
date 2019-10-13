using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Web.Http;

namespace XPike.Extensions.DependencyInjection.WebApi
{
    /// <summary>
    /// Extension methods for IServiceCollection to support using Microsoft.Extensions.DependencyInjection
    /// in ASP.Net WebAPI.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the API controllers to the service collection with a scoped lifetime.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ApiController)))
                    {
                        services.TryAddScoped(type);
                    }
                }
            }

            return services;
        }
    }
}
