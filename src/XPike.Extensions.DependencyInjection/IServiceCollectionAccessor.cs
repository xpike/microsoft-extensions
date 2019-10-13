using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// Interface for boxing IServiceCollection into a ReadonlyCollection.
    /// </summary>
    internal interface IServiceCollectionAccessor
    {
        /// <summary>
        /// Gets the service descriptors as a IReadonlyCollection.
        /// </summary>
        /// <returns>IReadOnlyCollection&lt;ServiceDescriptor&gt;.</returns>
        IReadOnlyCollection<ServiceDescriptor> GetServiceDescriptors();
    }
}
