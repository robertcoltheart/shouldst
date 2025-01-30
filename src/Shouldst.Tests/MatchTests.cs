namespace Shouldst.Tests;

public class MatchTests
{
    [Test]
    public void AssertMatchWithMatchingSucceeds()
    {
        "value".ShouldMatch(x => x == "value");
    }

    [Test]
    public void AssertMatchWithMatchingClassSucceeds()
    {
        var value = new MyRecord("value");

        value.ShouldMatch(x => x.Value == "value");
    }

    [Test]
    public void AssertMatchWithNotMatchingThrows()
    {
        Assert.Throws<ShouldException>(() => "value".ShouldMatch(x => x == "wrong"));
    }

    [Test]
    public void AssertMatchWithNotMatchingClassThrows()
    {
        var value = new MyRecord("value");

        Assert.Throws<ShouldException>(() => value.ShouldMatch(x => x.Value == "wrong"));
    }

    private record MyRecord(string Value);
}
