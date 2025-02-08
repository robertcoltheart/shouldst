using System.Text.RegularExpressions;

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

    [Test]
    public void AssertMatchWithMatchingRegexSucceeds()
    {
        "my value".ShouldMatch("my.*");
        "my value".ShouldMatch(new Regex("my.*"));
    }

    [Test]
    public void AssertMatchWithNonMatchingRegexThrows()
    {
        Assert.Throws<ShouldException>(() => "my value".ShouldMatch("wrong.*"));
        Assert.Throws<ShouldException>(() => "my value".ShouldMatch(new Regex("wrong.*")));
    }

    private record MyRecord(string Value);
}
