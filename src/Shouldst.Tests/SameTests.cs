namespace Shouldst.Tests;

public class SameTests
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
    public void AssertNotSameWithSameThrows()
    {
        var value = new object();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeTheSameAs(value));
    }

    [Test]
    public void AssertNotSameWithDifferentSucceeds()
    {
        var value = new object();

        value.ShouldNotBeTheSameAs(new object());
    }

    [Test]
    public void AssertSameWithSameValueTypeThrows()
    {
        Assert.Throws<ShouldException>(() => 1.ShouldBeTheSameAs(1));
    }

    [Test]
    public void AssertNotSameWithSameValueTypeSucceeds()
    {
        1.ShouldNotBeTheSameAs(1);
    }

    [Test]
    public void AssertSameWithDifferentValueTypeThrows()
    {
        Assert.Throws<ShouldException>(() => 1.ShouldBeTheSameAs(2));
    }

    [Test]
    public void AssertNotSameWithDifferentValueTypeSucceeds()
    {
        1.ShouldNotBeTheSameAs(2);
    }
}
