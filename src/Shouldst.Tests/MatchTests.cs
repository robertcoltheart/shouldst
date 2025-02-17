using System.Text.RegularExpressions;

namespace Shouldst.Tests;

public class MatchTests
{
    [Test]
    public void ShouldMatchWithMatchingSucceeds()
    {
        "value".ShouldMatch(x => x == "value");
    }

    [Test]
    public void ShouldMatchWithMatchingClassSucceeds()
    {
        var value = new MyRecord("value");

        value.ShouldMatch(x => x.Value == "value");
    }

    [Test]
    public void ShouldMatchWithNotMatchingThrows()
    {
        Assert.Throws<ShouldException>(() => "value".ShouldMatch(x => x == "wrong"));
    }

    [Test]
    public void ShouldMatchWithNotMatchingClassThrows()
    {
        var value = new MyRecord("value");

        Assert.Throws<ShouldException>(() => value.ShouldMatch(x => x.Value == "wrong"));
    }

    [Test]
    public void ShouldMatchWithMatchingRegexSucceeds()
    {
        "my value".ShouldMatch("my.*");
        "my value".ShouldMatch(new Regex("my.*"));
    }

    [Test]
    public void ShouldMatchWithNonMatchingRegexThrows()
    {
        Assert.Throws<ShouldException>(() => "my value".ShouldMatch("wrong.*"));
        Assert.Throws<ShouldException>(() => "my value".ShouldMatch(new Regex("wrong.*")));
    }

    private record MyRecord(string Value);
}
