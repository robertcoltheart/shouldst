namespace Shouldst.Tests;

public class BooleanTests
{
    [Test]
    public void AssertTrueWithTrueSucceeds()
    {
        true.ShouldBeTrue();
    }

    [Test]
    public void AsserTrueWithFalseThrows()
    {
        Assert.Throws<ShouldException>(() => false.ShouldBeTrue());
    }

    [Test]
    public void AssertFalseWithFalseSucceeds()
    {
        false.ShouldBeFalse();
    }

    [Test]
    public void AssertFalseWithTrueThrows()
    {
        Assert.Throws<ShouldException>(() => true.ShouldBeFalse());
    }

    [Test]
    [Arguments(true)]
    public void AssertTrueWithNullableTrueSucceeds(bool? value)
    {
        value.ShouldBeTrue();
    }

    [Test]
    [Arguments(null)]
    public void AssertTrueWithNullThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeTrue());
    }

    [Test]
    [Arguments(false)]
    public void AssertTrueWithNullableFalseThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeTrue());
    }

    [Test]
    [Arguments(true)]
    public void AssertFalseWithNullableTrueThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeFalse());
    }

    [Test]
    [Arguments(null)]
    public void AssertFalseWithNullThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeFalse());
    }

    [Test]
    [Arguments(false)]
    public void AssertFalseWithNullableFalseSucceeds(bool? value)
    {
        value.ShouldBeFalse();
    }
}
