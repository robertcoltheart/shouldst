namespace Shouldst.Tests;

public class SameTests
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
    public void ShouldNotBeSameWithSameThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeTheSameAs(value));
    }

    [Test]
    public void ShouldNotBeSameWithDifferentSucceeds()
    {
        var value = new object();

        value.ShouldNotBeTheSameAs(new object());
    }

    [Test]
    public void ShouldBeSameWithSameValueTypeThrows()
    {
        Assert.Throws<ShouldException>(() => 1.ShouldBeTheSameAs(1));
    }

    [Test]
    public void ShouldNotBeSameWithSameValueTypeSucceeds()
    {
        1.ShouldNotBeTheSameAs(1);
    }

    [Test]
    public void ShouldBeSameWithDifferentValueTypeThrows()
    {
        Assert.Throws<ShouldException>(() => 1.ShouldBeTheSameAs(2));
    }

    [Test]
    public void ShouldNotBeSameWithDifferentValueTypeSucceeds()
    {
        1.ShouldNotBeTheSameAs(2);
    }
}
