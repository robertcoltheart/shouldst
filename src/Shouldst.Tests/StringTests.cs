namespace Shouldst.Tests;

public class StringTests
{
    [Test]
    public void AssertContainsWithSubstringSucceeds()
    {
        "this value".ShouldContain("val");
    }

    [Test]
    public void AssertContainsWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldContain("missing"));
    }

    [Test]
    public void AssertNotContainsWithSubstringThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldNotContain("val"));
    }

    [Test]
    public void AssertNotContainsWithMissingSucceeds()
    {
        "this value".ShouldNotContain("missing");
    }

    [Test]
    public void AssertEqualsIgnoreCaseWithMatchingSucceeds()
    {
        "this".ShouldEqualIgnoringCase("THIS");
    }

    [Test]
    public void AssertEqualsIgnoreCaseWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this".ShouldEqualIgnoringCase("missing"));
    }

    [Test]
    public void ShouldStartWithStringWithSubstringSucceeds()
    {
        "this value".ShouldStartWith("this ");
    }

    [Test]
    public void ShouldStartWithStringWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldStartWith("missing"));
    }

    [Test]
    public void AssertEndsWithStringWithSubstringSucceeds()
    {
        "this value".ShouldEndWith(" value");
    }

    [Test]
    public void AssertEndsWithStringWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldEndWith("missing"));
    }

    [Test]
    public void AssertSurroundsWithMatchingSucceeds()
    {
        "'value'".ShouldBeSurroundedWith("'");
        "'value'".ShouldBeSurroundedWith("'", "'");
    }

    [Test]
    public void AssertSurroundsWithMissingThrows()
    {
        "'value'".ShouldBeSurroundedWith("@");
        "'value'".ShouldBeSurroundedWith("'", "@");
        "'value'".ShouldBeSurroundedWith("@", "'");
    }
}
