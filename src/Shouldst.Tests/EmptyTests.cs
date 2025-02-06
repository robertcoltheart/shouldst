using System.Collections;
using FluentAssertions;

namespace Shouldst.Tests;

public class EmptyTests
{
    [Test]
    public void ShouldBeEmptyEnumerableWithEmptySucceeds()
    {
        GetEmptyValues().ShouldBeEmpty();
        GetEmptyValuesNonGeneric().ShouldBeEmpty();
    }

    [Test]
    public void ShouldNotBeEmptyEnumerableWithEmptyThrows()
    {
        Assert.Throws<ShouldException>(() => GetEmptyValues().ShouldNotBeEmpty());
        Assert.Throws<ShouldException>(() => GetEmptyValuesNonGeneric().ShouldNotBeEmpty());
    }

    [Test]
    public void ShouldBeEmptyEnumerableWithNonEmptyThrows()
    {
        Assert.Throws<ShouldException>(() => GetValues().ShouldBeEmpty());
        Assert.Throws<ShouldException>(() => GetValuesNonGeneric().ShouldBeEmpty());
    }

    [Test]
    public void ShouldNotBeEmptyEnumerableWithNonEmptySucceeds()
    {
        GetValues().ShouldNotBeEmpty();
        GetValuesNonGeneric().ShouldNotBeEmpty();
    }

    [Test]
    public void ShouldBeEmptyStringWithEmptySucceeds()
    {
        string.Empty.ShouldBeEmpty();
    }

    [Test]
    public void ShouldNotBeEmptyWithEmptyThrows()
    {
        Assert.Throws<ShouldException>(() => string.Empty.ShouldNotBeEmpty());
    }

    [Test]
    public void ShouldBeEmptyWithStringThrows()
    {
        Assert.Throws<ShouldException>(() => "val".ShouldBeEmpty());
    }

    [Test]
    public void ShouldNotBeEmptyWithStringSucceeds()
    {
        "val".ShouldNotBeEmpty();
    }

    private IEnumerable<string> GetValues()
    {
        yield return "1";
        yield return "2";
    }

    private IEnumerable GetValuesNonGeneric()
    {
        return new[]
        {
            "1",
            "2"
        };
    }

    private IEnumerable<string> GetEmptyValues()
    {
        yield break;
    }

    private IEnumerable GetEmptyValuesNonGeneric()
    {
        return Array.Empty<string>();
    }
}
