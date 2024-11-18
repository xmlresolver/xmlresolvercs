using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Org.XmlResolver;
using Org.XmlResolver.Features;
using Org.XmlResolver.Utils;

namespace UnitTests {
    public class ResolverTestJar : BaseTestRoot {
        private XmlResolverConfiguration config = null;
        private Resolver resolver = null;

        [SetUp]
        public void Setup() {
            config = new XmlResolverConfiguration(new List<Uri>(), new List<string>());
            config.SetFeature(ResolverFeature.URI_FOR_SYSTEM, true);
            config.SetFeature(ResolverFeature.CACHE, null);
            config.SetFeature(ResolverFeature.CACHE_UNDER_HOME, false);
            config.SetFeature(ResolverFeature.ASSEMBLY_CATALOGS, "UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            resolver = new Resolver(config);
        }
        
        [Test]
        public void LookupSystem() {
            string systemId = "https://xmlresolver.org/ns/assembly-sample/sample.dtd";

            try {
                object stream = resolver.GetEntity(UriUtils.NewUri(systemId), null, typeof(Stream));
                Assert.That(stream, Is.Not.Null);
            } catch (Exception) {
                Assert.Fail();
            }
        }
    }
}
