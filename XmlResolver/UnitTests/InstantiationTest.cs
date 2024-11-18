using System;
using NUnit.Framework;
using Org.XmlResolver;

namespace UnitTests {
    public class InstantiationTest {
        [Test]
        public void InstantiateTest() {
            var handle = Activator.CreateInstance("XmlResolver", "Org.XmlResolver.Resolver");
            Resolver resolver = (Resolver) handle.Unwrap();
            Assert.That(resolver, Is.Not.Null);
        }
    }
}
