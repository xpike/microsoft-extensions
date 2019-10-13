using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XPike.Extensions.DependencyInjection.WebApi;

namespace XPike.Extensions.DependencyInjection.WebApi.Tests
{
    [TestClass]
    public class DependencyResolverTests
    {
        [TestMethod]
        public void BeginScope_should_return_a_scoped_container()
        {
            var services = new ServiceCollection();
            services.AddScoped<IFoo, Foo>();

            var provider = services.BuildServiceProvider();
            var resolver = new MicrosoftDependencyResolver(provider);

            using (var scope = resolver.BeginScope())
            {
                var foo = scope.GetService(typeof(IFoo)) as IFoo;
                int i = foo.Increment();
                Assert.AreEqual(1, i);
            }

            var bar = provider.GetService(typeof(IFoo)) as IFoo;
            int b = bar.Increment();
            Assert.AreEqual(1, b);
        }
    }

    public interface IFoo
    {
        int Increment();
    }

    public class Foo : IFoo
    {
        int bar = 0;

        public int Increment()
        {
            return ++bar;
        }
    }
}
