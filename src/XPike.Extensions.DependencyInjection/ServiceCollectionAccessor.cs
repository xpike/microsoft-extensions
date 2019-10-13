using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// Boxes IServiceCollection
    /// Implements the <see cref="XPike.Extensions.DependencyInjection.IServiceCollectionAccessor" />
    /// </summary>
    /// <seealso cref="XPike.Extensions.DependencyInjection.IServiceCollectionAccessor" />
    /// <remarks>
    /// The service collection is needed to verfify the provider, so it must be available after the IServiceProvider
    /// is build. To do this, we add the collection to itself during registration. This accessor class is desiged 
    /// to help protect IServiceCollection from access outside of the Verify() method.
    /// </remarks>
    internal class ServiceCollectionAccessor : IServiceCollectionAccessor
    {
        private IServiceCollection services;

        internal ServiceCollectionAccessor(IServiceCollection services)
        {
            this.services = services;
        }

        public IReadOnlyCollection<ServiceDescriptor> GetServiceDescriptors()
        {
            return new ReadOnlyCollection<ServiceDescriptor>(services);
        }
    }
}
