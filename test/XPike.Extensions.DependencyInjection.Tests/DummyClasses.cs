using System;
using System.Collections.Generic;
using System.Text;

namespace XPike.Extensions.DependencyInjection.Tests
{
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

    public interface IBar
    {
        int TheNumber { get; }
    }

    public class Bar : IBar
    {
        IFoo foo;
        public Bar(IFoo foo)
        {
            this.foo = foo;
        }

        public int TheNumber { get; }
    }
}
