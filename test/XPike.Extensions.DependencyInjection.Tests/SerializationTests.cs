using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace XPike.Extensions.DependencyInjection.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Serialize_the_exception()
        {
            IList<VerificationResult> results = new List<VerificationResult> {
                new VerificationResult(new ServiceDescriptor(typeof(IFoo), typeof(Foo), ServiceLifetime.Singleton),
                                        new Exception("test"), "test message")

            };

            var e = new ServiceProviderVerificationException(results);

            var serializer = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, e);

                ms.Position = 0;

                var e2 = (ServiceProviderVerificationException)serializer.Deserialize(ms);

                Assert.AreEqual("test", e2.Results.First().Exception.Message);
            }
        }
    }
}
