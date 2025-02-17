namespace Shouldst.Tests;

public class ReferenceTests
{
    [Test]
    public void ShouldBeSameWithSameSucceeds()
    {
        var value = new object();

        value.ShouldBeTheSameAs(value);
    }

    [Test]
    public void ShouldBeSameWithDifferentThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldBeTheSameAs(new object()));
    }

    [Test]
    public void ShouldBeSameWithNullThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldBeTheSameAs(null));
    }

    [Test]
    public void ShouldBeDifferentWithSameThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeTheSameAs(value));
    }

    [Test]
    public void ShouldBeDifferentWithDifferentSucceeds()
    {
        var value = new object();

        value.ShouldNotBeTheSameAs(new object());
    }

    [Test]
    public void ShouldBeDifferentWithNullSucceeds()
    {
        var value = new object();

        value.ShouldNotBeTheSameAs(null);
    }

    [Test]
    public void ShouldBeSameWithBothNullSucceeds()
    {
        var value = default(object);

        value.ShouldBeTheSameAs(null);
    }
}
