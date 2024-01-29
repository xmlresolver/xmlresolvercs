using System.Text;
using XmlResolver;
using XmlResolver.Utils;

namespace Tests;

public class DataUriTest : XmlResolverTest
{
    private XmlResolver.XmlResolver _resolver = null;

    [SetUp]
    public void Setup()
    {
        XmlResolverConfiguration config = new XmlResolverConfiguration(UriUtils
            .Resolve(TestRootDirectory, "Tests/resources/datauri.xml").ToString());
        _resolver = new XmlResolver.XmlResolver(config);
    }

    [Test]
    public void TestDataUriText()
    {
        string href = "example.txt";
        string baseuri = "http://example.com/";

        string line = null;
        var req = _resolver.GetRequest(href, baseuri);
        var result = _resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            using (StreamReader reader = new StreamReader(result.Stream))
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

        string line = null;
        var req = _resolver.GetRequest(href, baseuri);
        var result = _resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            using (StreamReader reader = new StreamReader(result.Stream))
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

        string line = null;
        var req = _resolver.GetRequest(href, baseuri);
        var result = _resolver.Resolve(req);
        if (result.Stream == null)
        {
            Assert.Fail();
        }
        else
        {
            using (StreamReader reader = new StreamReader(result.Stream))
            {
                line = reader.ReadLine();
            }

            Assert.That("<doc>I was a data URI</doc>", Is.EqualTo(line));
        }
    }
}