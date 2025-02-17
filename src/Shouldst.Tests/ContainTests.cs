using System.Collections;

namespace Shouldst.Tests;

public class ContainTests
{
    [Test]
    public void ShouldContainWithMatchingPrimitiveValueSucceeds()
    {
        GetPrimitiveValues().ShouldContain(1, 2);
        GetNonGeneric(GetPrimitiveValues()).ShouldContain(1, 2);
    }

    [Test]
    public void ShouldNotContainWithMatchingPrimitiveValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetPrimitiveValues().ShouldNotContain(1, 2));
        Assert.Throws<ShouldException>(() => GetNonGeneric(GetPrimitiveValues()).ShouldNotContain(1, 2));
    }

    [Test]
    public void ShouldContainWithMissingPrimitiveValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetPrimitiveValues().ShouldContain(10, 11));
        Assert.Throws<ShouldException>(() => GetNonGeneric(GetPrimitiveValues()).ShouldContain(10, 11));
    }

    [Test]
    public void ShouldNotContainWithMissingPrimitiveValueSucceeds()
    {
        GetPrimitiveValues().ShouldNotContain(10, 11);
        GetNonGeneric(GetPrimitiveValues()).ShouldNotContain(10, 11);
    }

    [Test]
    public void ShouldContainWithMatchingComplexValueSucceeds()
    {
        GetComplexValues().ShouldContain(new MyRecord("val1", 1), new MyRecord("val2", 2));
        GetNonGeneric(GetComplexValues()).ShouldContain(new MyRecord("val1", 1), new MyRecord("val2", 2));
    }

    [Test]
    public void ShouldNotContainWithMatchingComplexValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldNotContain(new MyRecord("val1", 1), new MyRecord("val2", 2)));
        Assert.Throws<ShouldException>(() => GetNonGeneric(GetComplexValues()).ShouldNotContain(new MyRecord("val1", 1), new MyRecord("val2", 2)));
    }

    [Test]
    public void ShouldContainWithMissingComplexValueThrows()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldContain(new MyRecord("val10", 10), new MyRecord("val11", 11)));
        Assert.Throws<ShouldException>(() => GetNonGeneric(GetComplexValues()).ShouldContain(new MyRecord("val10", 10), new MyRecord("val11", 11)));
    }

    [Test]
    public void ShouldNotContainWithMissingComplexValueSucceeds()
    {
        GetComplexValues().ShouldNotContain(new MyRecord("val10", 10), new MyRecord("val11", 11));
        GetNonGeneric(GetComplexValues()).ShouldNotContain(new MyRecord("val10", 10), new MyRecord("val11", 11));
    }

    [Test]
    public void ShouldContainWithMatchingExpressionSucceeds()
    {
        GetComplexValues().ShouldContain(x => x.IntValue == 1);
    }

    [Test]
    public void ShouldNotContainWithMatchingExpressionThrows()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldNotContain(x => x.IntValue == 1));
    }

    [Test]
    public void ShouldContainWithMissingExpressionThrows()
    {
        Assert.Throws<ShouldException>(() => GetComplexValues().ShouldContain(x => x.IntValue == 11));
    }

    [Test]
    public void ShouldNotContainWithMissingExpressionSucceeds()
    {
        GetComplexValues().ShouldNotContain(x => x.IntValue == 11);
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

    private IEnumerable GetNonGeneric<T>(IEnumerable<T> values)
    {
        return values;
    }

    private record MyRecord(string Value, int IntValue);
}
