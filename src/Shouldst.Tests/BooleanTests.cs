namespace Shouldst.Tests;

public class BooleanTests
{
    [Test]
    public void ShouldBeTrueWithTrueSucceeds()
    {
        true.ShouldBeTrue();
    }

    [Test]
    public void ShouldBeTrueWithFalseThrows()
    {
        Assert.Throws<ShouldException>(() => false.ShouldBeTrue());
    }

    [Test]
    public void ShouldBeFalseWithFalseSucceeds()
    {
        false.ShouldBeFalse();
    }

    [Test]
    public void ShouldBeFalseWithTrueThrows()
    {
        Assert.Throws<ShouldException>(() => true.ShouldBeFalse());
    }

    [Test]
    [Arguments(true)]
    public void ShouldBeTrueWithNullableTrueSucceeds(bool? value)
    {
        value.ShouldBeTrue();
    }

    [Test]
    [Arguments(null)]
    public void ShouldBeTrueWithNullThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeTrue());
    }

    [Test]
    [Arguments(false)]
    public void ShouldBeTrueWithNullableFalseThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeTrue());
    }

    [Test]
    [Arguments(true)]
    public void ShouldBeFalseWithNullableTrueThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeFalse());
    }

    [Test]
    [Arguments(null)]
    public void ShouldBeFalseWithNullThrows(bool? value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldBeFalse());
    }

    [Test]
    [Arguments(false)]
    public void ShouldBeFalseWithNullableFalseSucceeds(bool? value)
    {
        value.ShouldBeFalse();
    }
}
