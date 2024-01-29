using System.Text;
using XmlResolver;
using XmlResolver.Utils;

namespace Tests;

public class DataUriTest : XmlResolverTest
{
    private XmlResolver.XmlResolver? _resolver = null;
    private XmlResolver.XmlResolver Resolver
    {
        get
        {
            if (_resolver == null)
            {
                var config = new XmlResolverConfiguration(UriUtils
                    .Resolve(TestRootDirectory, "Tests/resources/datauri.xml").ToString());
                _resolver = new XmlResolver.XmlResolver(config);
            }

            return _resolver;
        }
    }

    [Test]
    public void TestDataUriText()
    {
        string href = "example.txt";
        string baseuri = "http://example.com/";

        var req = Resolver.GetRequest(href, baseuri);
        var result = Resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            string? line;
            using (var reader = new StreamReader(result.Stream))
            {
                line = reader.ReadLine();
            }
            Assert.That("A short note.", Is.EqualTo(line));
        }
    }

    [Test]
    public void TestDataUriCharset()
    {
        string href = "greek.txt";
        string baseuri = "http://example.com/";

        // We don't do this in the resolver, but we do it here to demonstrate that
        // it will work in applications that use the resolver.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var req = Resolver.GetRequest(href, baseuri);
        var result = Resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            string? line;
            using (var reader = new StreamReader(result.Stream))
            {
                line = reader.ReadLine();
            }

            Assert.That("ΎχΎ", Is.EqualTo(line));
        }
    }

    [Test]
    public void TestDataUriEncoded()
    {
        string href = "example.xml";
        string baseuri = "http://example.com/";

        var req = Resolver.GetRequest(href, baseuri);
        var result = Resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            string? line;
            using (StreamReader reader = new StreamReader(result.Stream))
            {
                line = reader.ReadLine();
            }

            Assert.That("<doc>I was a data URI</doc>", Is.EqualTo(line));
        }
    }
}