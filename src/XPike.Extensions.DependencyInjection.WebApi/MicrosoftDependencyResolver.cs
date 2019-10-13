using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace XPike.Extensions.DependencyInjection.WebApi
{
    /// <summary>
    /// Class MicrosoftDependencyResolver. This class cannot be inherited.
    /// Implements the <see cref="System.Web.Http.Dependencies.IDependencyResolver" />
    /// </summary>
    /// <seealso cref="System.Web.Http.Dependencies.IDependencyResolver" />
    public sealed class MicrosoftDependencyResolver : IDependencyResolver
    {
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public MicrosoftDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "It is up to the consumer to dispose of the scope.")]
        public IDependencyScope BeginScope()
        {
            var scopedProvider = serviceProvider.CreateScope().ServiceProvider;
            return new MicrosoftDependencyResolver(scopedProvider);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)serviceProvider).Dispose();
        }

        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>The retrieved service.</returns>
        public object GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>The retrieved collection of services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return serviceProvider.GetServices(serviceType);
        }
    }
}
