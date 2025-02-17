namespace Shouldst.Tests;

public class NullTests
{
    [Test]
    public void ShouldBeNullWithNullSucceeds()
    {
        var value = default(object);

        value.ShouldBeNull();
    }

    [Test]
    public void ShouldBeNullWithNotNullThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldBeNull());
    }

    [Test]
    public void ShouldNotBeNullWithNullThrows()
    {
        var value = default(object);

        Assert.Throws<ShouldException>(() => value.ShouldNotBeNull());
    }

    [Test]
    public void ShouldNotBeNullWithNotNullSucceeds()
    {
        var value = new object();

        value.ShouldNotBeNull();
    }
}
