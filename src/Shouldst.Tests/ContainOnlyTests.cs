namespace Shouldst.Tests;

public class ContainOnlyTests
{
    [Test]
    public void ShouldContainOnlyWithMatchingPrimitiveValueSucceeds()
    {
        GetPrimitiveValues().ShouldContainOnly(1, 2, 3);
    }

    [Test]
    public void ShouldContainOnlyWithMissingPrimitiveValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetPrimitiveValues().ShouldContainOnly(1, 2));
    }

    [Test]
    public void ShouldContainOnlyWithExtraPrimitiveValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetPrimitiveValues().ShouldContainOnly(1, 2, 3, 4));
    }

    [Test]
    public void ShouldContainOnlyWithMatchingComplexValueSucceeds()
    {
        GetComplexValues().ShouldContainOnly(new MyRecord("val1", 1), new MyRecord("val2", 2), new MyRecord("val3", 3));
    }

    [Test]
    public void ShouldContainOnlyWithMissingComplexValueSucceeds()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldContainOnly(new MyRecord("val1", 1), new MyRecord("val2", 2)));
    }

    [Test]
    public void ShouldContainOnlyWithExtraComplexValueSucceeds()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldContainOnly(new MyRecord("val1", 1), new MyRecord("val2", 2), new MyRecord("val3", 3), new MyRecord("val4", 4)));
    }

    private IEnumerable<int> GetPrimitiveValues()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    private IEnumerable<MyRecord> GetComplexValues()
    {
        yield return new MyRecord("val1", 1);
        yield return new MyRecord("val2", 2);
        yield return new MyRecord("val3", 3);
    }

    private record MyRecord(string Value, int IntValue);
}
