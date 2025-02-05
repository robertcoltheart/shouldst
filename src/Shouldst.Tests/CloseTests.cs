namespace Shouldst.Tests;

public class CloseTests
{
    [Test]
    public void AssertCloseToWithCloseSucceeds()
    {
        1f.ShouldBeCloseTo(1f);
        1d.ShouldBeCloseTo(1d);
        1m.ShouldBeCloseTo(1m);
    }

    [Test]
    public void AssertCloseToWithFarAwayThrows()
    {
        Assert.Throws<ShouldException>(() => 1f.ShouldBeCloseTo(2f));
        Assert.Throws<ShouldException>(() => 1d.ShouldBeCloseTo(2d));
        Assert.Throws<ShouldException>(() => 1m.ShouldBeCloseTo(2m));
    }

    [Test]
    public void AssertCloseToWithCloseToleranceSucceeds()
    {
        1f.ShouldBeCloseTo(1.1f, 0.15f);
        1d.ShouldBeCloseTo(1.1d, 0.15d);
        1m.ShouldBeCloseTo(1.1m, 0.15m);
    }

    [Test]
    public void AssertCloseToWithFarAwayToleranceThrows()
    {
        Assert.Throws<ShouldException>(() => 1f.ShouldBeCloseTo(2f, 0.15f));
        Assert.Throws<ShouldException>(() => 1d.ShouldBeCloseTo(2d, 0.15d));
        Assert.Throws<ShouldException>(() => 1m.ShouldBeCloseTo(2m, 0.15m));
    }
}
