namespace Shouldst.Tests;

public class MatchAllTests
{
    [Test]
    public void ShouldAllBeWithMatchingItemsSucceeds()
    {
        GetValues().ShouldAllBe(x => x.IntValue > 0 && x.Value != "wrong");
    }

    [Test]
    public void ShouldAllBeWithFailingItemThrows()
    {
        Assert.Throws<ShouldException>(() => GetValues().ShouldAllBe(x => x.IntValue == 1));
    }

    private IEnumerable<MyRecord> GetValues()
    {
        yield return new MyRecord("val1", 1);
        yield return new MyRecord("val2", 2);
    }

    private record MyRecord(string Value, int IntValue);
}
