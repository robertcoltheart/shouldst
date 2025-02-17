namespace Shouldst.Tests;

public class CloseTests
{
    [Test]
    public void ShouldBeCloseToWithCloseSucceeds()
    {
        1f.ShouldBeCloseTo(1f);
        1d.ShouldBeCloseTo(1d);
        1m.ShouldBeCloseTo(1m);
    }

    [Test]
    public void ShouldBeCloseToWithFarAwayThrows()
    {
        Assert.Throws<ShouldException>(() => 1f.ShouldBeCloseTo(2f));
        Assert.Throws<ShouldException>(() => 1d.ShouldBeCloseTo(2d));
        Assert.Throws<ShouldException>(() => 1m.ShouldBeCloseTo(2m));
    }

    [Test]
    public void ShouldBeCloseToWithCloseToleranceSucceeds()
    {
        1f.ShouldBeCloseTo(1.1f, 0.15f);
        1d.ShouldBeCloseTo(1.1d, 0.15d);
        1m.ShouldBeCloseTo(1.1m, 0.15m);
        TimeSpan.FromSeconds(1).ShouldBeCloseTo(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(0.1));
        DateTime.Parse("2025-02-05T12:30:00Z").ShouldBeCloseTo(DateTime.Parse("2025-02-05T12:30:01Z"), TimeSpan.FromSeconds(5));
    }

    [Test]
    public void ShouldBeCloseToWithFarAwayToleranceThrows()
    {
        Assert.Throws<ShouldException>(() => 1f.ShouldBeCloseTo(2f, 0.15f));
        Assert.Throws<ShouldException>(() => 1d.ShouldBeCloseTo(2d, 0.15d));
        Assert.Throws<ShouldException>(() => 1m.ShouldBeCloseTo(2m, 0.15m));
        Assert.Throws<ShouldException>(() => TimeSpan.FromSeconds(1).ShouldBeCloseTo(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0.1)));
        Assert.Throws<ShouldException>(() => DateTime.Parse("2025-02-05T12:30:00Z").ShouldBeCloseTo(DateTime.Parse("2025-02-05T12:50:01Z"), TimeSpan.FromSeconds(10)));
    }
}
