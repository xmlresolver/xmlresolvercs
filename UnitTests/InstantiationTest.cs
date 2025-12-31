using System;
using NUnit.Framework;
using XmlResolver;

namespace UnitTests {
    public class InstantiationTest {
        [Test]
        public void InstantiateTest() {
            var handle = Activator.CreateInstance("XmlResolver", "XmlResolver.XmlResolver");
            Assert.IsNotNull(handle);
            XmlResolver.XmlResolver resolver = (XmlResolver.XmlResolver) handle.Unwrap();
            Assert.NotNull(resolver);
        }
    }
}