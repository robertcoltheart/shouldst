namespace Shouldst.Tests;

public class ReferenceTests
{
    [Test]
    public void AssertSameWithSameSucceeds()
    {
        var value = new object();

        value.ShouldBeTheSameAs(value);
    }

    [Test]
    public void AssertSameWithDifferentThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldBeTheSameAs(new object()));
    }

    [Test]
    public void AssertDifferentWithSameThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeTheSameAs(value));
    }

    [Test]
    public void AssertDifferentWithDifferentSucceeds()
    {
        var value = new object();

        value.ShouldNotBeTheSameAs(new object());
    }
}
