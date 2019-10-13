using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace XPike.Extensions.DependencyInjection.Tests
{
    [TestClass]
    public class Verify_Tests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            FieldInfo isVerified = typeof(IServiceProviderExtensions)
                .GetField("isVerified", BindingFlags.NonPublic | BindingFlags.Static);

            isVerified.SetValue(typeof(IServiceProviderExtensions), false);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            FieldInfo isVerified = typeof(IServiceProviderExtensions)
                .GetField("isVerified", BindingFlags.NonPublic | BindingFlags.Static);

            isVerified.SetValue(typeof(IServiceProviderExtensions), false);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_throw_exception_if_IServiceCollection_not_registered()
        {
            IServiceCollection services = new ServiceCollection();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_throw_exception_if_already_verified()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();

            provider.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceProviderVerificationException))]
        public void Should_throw_exception_if_singlton_depends_on_transient()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            services.AddSingleton<IBar, Bar>();
            services.AddTransient<IFoo, Foo>();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceProviderVerificationException))]
        public void Should_throw_exception_if_singlton_depends_on_scoped()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            services.AddSingleton<IBar, Bar>();
            services.AddScoped<IFoo, Foo>();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }


        [TestMethod]
        [ExpectedException(typeof(ServiceProviderVerificationException))]
        public void Should_throw_exception_if_scoped_depends_on_transient()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            services.AddScoped<IBar, Bar>();
            services.AddTransient<IFoo, Foo>();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceProviderVerificationException))]
        public void Should_throw_exception_if_missing_registration()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            services.AddSingleton<IBar, Bar>();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }

        [TestMethod]
        public void Should_succeed_with_proper_container()
        {
            IServiceCollection services = new ServiceCollection().AddServiceProviderVerification();
            services.AddSingleton<IBar, Bar>();
            services.AddSingleton<IFoo, Foo>();
            IServiceProvider provider = services.BuildServiceProvider();
            provider.Verify();
        }
    }
}
