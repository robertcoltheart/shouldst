namespace Shouldst.Tests;

public class StringTests
{
    [Test]
    public void ShouldContainWithSubstringSucceeds()
    {
        "this value".ShouldContain("val");
    }

    [Test]
    public void ShouldContainWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldContain("missing"));
    }

    [Test]
    public void ShouldNotContainWithSubstringThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldNotContain("val"));
    }

    [Test]
    public void ShouldNotContainWithMissingSucceeds()
    {
        "this value".ShouldNotContain("missing");
    }

    [Test]
    public void ShouldEqualIgnoreCaseWithMatchingSucceeds()
    {
        "this".ShouldEqualIgnoringCase("THIS");
    }

    [Test]
    public void ShouldEqualIgnoreCaseWithMissingThrows()
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
    public void ShouldEndWithStringWithSubstringSucceeds()
    {
        "this value".ShouldEndWith(" value");
    }

    [Test]
    public void ShouldEndWithStringWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "this value".ShouldEndWith("missing"));
    }

    [Test]
    public void ShouldBeSurroundedWithMatchingSucceeds()
    {
        "'value'".ShouldBeSurroundedWith("'");
        "'value'".ShouldBeSurroundedWith("'", "'");
    }

    [Test]
    public void ShouldBeSurroundedWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => "'value'".ShouldBeSurroundedWith("@"));
        Assert.Throws<ShouldException>(() => "'value'".ShouldBeSurroundedWith("'", "@"));
        Assert.Throws<ShouldException>(() => "'value'".ShouldBeSurroundedWith("@", "'"));
    }
}
