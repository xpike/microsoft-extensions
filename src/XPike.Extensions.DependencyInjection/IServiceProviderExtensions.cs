using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XPike.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for IServiceProvider, to enhance the features and functionality of 
    /// Microsoft.Extensions.DependencyInjection.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        private static bool isVerified;
        private static readonly object verifyLock = new object();


        /// <summary>
        /// Verifies the dependency graph for completeness and valid dependency lifetimes.
        /// This method is thread-safe.
        /// </summary>
        /// <param name="provider">This IServiceProvider instance.</param>
        /// <exception cref="InvalidOperationException">The container has already been verified.</exception>
        /// <exception cref="InvalidOperationException">
        /// IServiceCollection must be registerd in order to verify the provider. 
        /// <c>Verify()</c> needs to obtain the list of registered services from the IServiceCollection instance.
        /// </exception>
        /// <exception cref="ServiceProviderVerificationException">The container is invalid.</exception>
        /// <remarks>
        /// This method also ensures that singletons only depend on other singletons and that scoped objects only
        /// depend on singletons and other scoped objects. Having a singleton depend on a transient object
        /// effectively makes that transient object a singleton and could result in undesired behavior.
        /// </remarks>
        /// <example>
        /// <code>
        /// public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        /// {
        ///     app.ApplicationServices.Verify();
        ///     ...
        ///     app.UseMvc();
        /// }
        /// </code>
        /// </example>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Custom exceptions are thrown with any related exception as the inner exception.")]
        public static void Verify(this IServiceProvider provider)
        {
            lock (verifyLock)
            {
                if (isVerified)
                    throw new InvalidOperationException("The container has already been verified.");

                IReadOnlyCollection<ServiceDescriptor> services = null;
                try
                {
                    services = provider.GetService<IServiceCollectionAccessor>()?.GetServiceDescriptors();
                }
                catch 
                { 
                    // Left empty on purpose. The inability to resolve IServiceCollection properly is handled
                    // below.
                }
                
                if (services == null)
                    throw new InvalidOperationException("An IServiceCollectionAccessor must be registerd in order to verify the provider. Verify needs to obtain the list of registered services from the IServiceCollection instance. Make sure you added the verify extensions:  services.AddServiceProviderVerification();");

                var errors = new List<VerificationResult>();
                
                // Create a scope so scoped registrations can be resolved.
                using (var scope = provider.CreateScope())
                {
                    foreach (ServiceDescriptor currentDescriptor in services)
                    {
                        if (currentDescriptor.Lifetime != ServiceLifetime.Transient)
                        {
                            errors.AddRange(VerifyScope(services, currentDescriptor));
                        }

                        try
                        {
                            // HACK: Open generics can't be resolved without providing a type parameter, so we need to skip them.
                            // This shouldn't be an issue, since these are not likely to be root objects, but
                            // rather dependencies of root objects such as controller and service objects. 
                            if (currentDescriptor.ServiceType.IsGenericType && currentDescriptor.ServiceType.GenericTypeArguments.Length == 0)
                                continue;

                            scope.ServiceProvider.GetService(currentDescriptor.ServiceType);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(new VerificationResult(currentDescriptor, ex));
                        }
                    }
                }

                if (errors.Count > 0)
                    throw new ServiceProviderVerificationException(errors.ToArray());

                isVerified = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We want to add the exception to the result regardless of the exception type.")]
        private static IList<VerificationResult> VerifyScope(IReadOnlyCollection<ServiceDescriptor> services, ServiceDescriptor currentDescriptor)
        {
            var errors = new List<VerificationResult>();

            if (currentDescriptor.ImplementationType != null)
            {
                try
                {
                    foreach (ConstructorInfo ctor in currentDescriptor.ImplementationType.GetConstructors())
                    {
                        foreach (ParameterInfo parameter in ctor.GetParameters())
                        {
                            var paramDescriptor = services.First(d => d.ServiceType == parameter.ParameterType);
                            if (paramDescriptor.Lifetime == ServiceLifetime.Transient ||
                                (currentDescriptor.Lifetime == ServiceLifetime.Singleton && 
                                 paramDescriptor.Lifetime == ServiceLifetime.Scoped))
                            {
                                errors.Add(new VerificationResult(currentDescriptor, null, $"An object with a lifetime of {paramDescriptor.Lifetime} cannot be a dependency of an object with a lifetime of {currentDescriptor.Lifetime}"));
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new VerificationResult(currentDescriptor, ex, $"Unable to determine dependencies for type {currentDescriptor.ServiceType} with scope {currentDescriptor.Lifetime}"));
                }
            }
            return errors;
        }
    }
}
