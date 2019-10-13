using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for IServiceCollection.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the service collection so it can later be verfied by the service provider.
        /// </summary>
        /// <param name="services">This IServicesCollection instance.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddServiceProviderVerification(this IServiceCollection services)
        {
            services.TryAddSingleton<IServiceCollectionAccessor>(new ServiceCollectionAccessor(services));
            return services;
        }
    }
}
