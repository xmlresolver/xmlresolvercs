namespace Tests;

public class InstantiationTest {
    [Test]
    public void InstantiateTest() {
        var handle = Activator.CreateInstance("XmlResolver", "XmlResolver.XmlResolver");
        XmlResolver.XmlResolver resolver = (XmlResolver.XmlResolver) handle.Unwrap();
        Assert.That(resolver, Is.Not.Null);
    }
}
