namespace Shouldst.Tests;

public class NullTests
{
    [Test]
    public void AssertNullWithNullSucceeds()
    {
        var value = default(object);

        value.ShouldBeNull();
    }

    [Test]
    public void AssertNullWithNotNullThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldBeNull());
    }

    [Test]
    public void AssertNotNullWithNullThrows()
    {
        var value = default(object);

        Assert.Throws<ShouldException>(() => value.ShouldNotBeNull());
    }

    [Test]
    public void AssertNotNullWithNotNullSucceeds()
    {
        var value = new object();

        value.ShouldNotBeNull();
    }
}
