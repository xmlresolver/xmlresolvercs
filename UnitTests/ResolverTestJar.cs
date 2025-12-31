using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using XmlResolver;
using XmlResolver.Features;
using XmlResolver.Utils;

namespace UnitTests {
    public class ResolverTestJar : BaseTestRoot {
        private XmlResolverConfiguration config = null;
        private XmlResolver.XmlResolver resolver = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            resolver = new XmlResolver.XmlResolver(config);
        }
        
        [Test]
        public void LookupSystem() {
            string systemId = "https://xmlresolver.org/ns/assembly-sample/sample.dtd";

            try {
                var stream = resolver.GetXmlResolver().GetEntity(UriUtils.NewUri(systemId), null, typeof(Stream));
                Assert.NotNull(stream);
            } catch (Exception) {
                Assert.Fail();
            }
        }
    }
}