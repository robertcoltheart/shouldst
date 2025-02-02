namespace Shouldst.Tests;

public class EqualTests
{
    [Test]
    [MethodDataSource(nameof(GetPrimitiveTypes))]
    public void AssertEqualWithEqualPrimitiveTypesSucceeds(object value)
    {
        value.ShouldBe(value);
        value.ShouldEqual(value);
    }

    [Test]
    [MethodDataSource(nameof(GetPrimitiveTypes))]
    public void AssertEqualWithNotEqualPrimitiveTypesThrows(object actual, object expected)
    {
        Assert.Throws<ShouldException>(() => actual.ShouldBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldEqual(expected));
    }

    [Test]
    [MethodDataSource(nameof(GetPrimitiveTypes))]
    public void AssertNotEqualWithEqualPrimitiveTypesThrows(object value)
    {
        Assert.Throws<ShouldException>(() => value.ShouldNotBe(value));
        Assert.Throws<ShouldException>(() => value.ShouldNotEqual(value));
    }

    [Test]
    [MethodDataSource(nameof(GetPrimitiveTypes))]
    public void AssertNotEqualWithNotEqualPrimitiveTypesThrows(object actual, object expected)
    {
        actual.ShouldNotBe(expected);
        actual.ShouldNotEqual(expected);
    }

    [Test]
    public void AssertEqualWithEqualCustomTypeSucceeds()
    {
        var actual = new SimpleRecord("value", 1);
        var expected = new SimpleRecord("value", 1);

        actual.ShouldBe(expected);
        actual.ShouldEqual(expected);
    }

    [Test]
    public void AssertEqualWithNotEqualCustomTypeThrows()
    {
        var actual = new SimpleRecord("value", 1);
        var expected = new SimpleRecord("value2", 2);

        Assert.Throws<ShouldException>(() => actual.ShouldBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldEqual(expected));
    }

    [Test]
    public void AssertNotEqualWithEqualCustomTypeThrows()
    {
        var actual = new SimpleRecord("value", 1);
        var expected = new SimpleRecord("value", 1);

        Assert.Throws<ShouldException>(() => actual.ShouldNotBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldNotEqual(expected));
    }

    [Test]
    public void AssertNotEqualWithNotEqualCustomTypesThrows()
    {
        var actual = new SimpleRecord("value", 1);
        var expected = new SimpleRecord("value2", 2);

        actual.ShouldNotBe(expected);
        actual.ShouldNotEqual(expected);
    }

    [Test]
    [MethodDataSource(nameof(GetEqualEnumerableTypes))]
    public void AssertEqualWithEqualEnumerableTypesSucceeds(object[] actual, object[] expected)
    {
        actual.ShouldBe(expected);
        actual.ShouldEqual(expected);
    }

    [Test]
    [MethodDataSource(nameof(GetEqualEnumerableTypes))]
    public void AssertNotEqualWithEqualEnumerableTypesThrows(object[] actual, object[] expected)
    {
        Assert.Throws<ShouldException>(() => actual.ShouldNotBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldNotEqual(expected));
    }

    [Test]
    [MethodDataSource(nameof(GetNotEqualEnumerableTypes))]
    public void AssertEqualWithNotEqualEnumerableTypesThrows(object[] actual, object[] expected)
    {
        Assert.Throws<ShouldException>(() => actual.ShouldBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldEqual(expected));
    }

    [Test]
    [MethodDataSource(nameof(GetNotEqualEnumerableTypes))]
    public void AssertNotEqualWithNotEqualEnumerableTypesSucceeds(object[] actual, object[] expected)
    {
        actual.ShouldNotBe(expected);
        actual.ShouldNotEqual(expected);
    }

    [Test]
    public void AssertEqualWithEqualObjectsSucceeds()
    {
        var value = new object();

        value.ShouldBe(value);
        value.ShouldEqual(value);
    }

    [Test]
    public void AssertNotEqualWithNotEqualObjectsSucceeds()
    {
        var value = new object();

        value.ShouldNotBe(new object());
        value.ShouldNotEqual(new object());
    }

    [Test]
    public void AssertEqualWithUnevenEnumerableTypesThrows()
    {
        var actual = new[] { 1, 2, 3 };
        var expected = new[] { 1, 2 };

        Assert.Throws<ShouldException>(() => actual.ShouldBe(expected));
        Assert.Throws<ShouldException>(() => actual.ShouldEqual(expected));
    }

    [Test]
    public void AssertNotEqualWithUnevenEnumerableTypesSucceeds()
    {
        var actual = new[] { 1, 2, 3 };
        var expected = new[] { 1, 2 };

        actual.ShouldNotBe(expected);
        actual.ShouldNotEqual(expected);
    }

    public static IEnumerable<Func<(object, object)>> GetPrimitiveTypes()
    {
        yield return () => (1, 2);
        yield return () => (1u, 2u);
        yield return () => (123.123f, 123.456f);
        yield return () => (123.123d, 123.456d);
        yield return () => (123.123m, 123.456m);
        yield return () => ("value", "value2");
        yield return () => (new DateTime(2025, 12, 25), new DateTime(2025, 12, 26));
        yield return () => (new TimeOnly(15, 10, 11), new TimeOnly(15, 10, 12));
    }

    public static IEnumerable<Func<(object[], object[])>> GetEqualEnumerableTypes()
    {
        yield return () => ([1, 2, 3], [1, 2, 3]);
        yield return () => (["value1", "value2"], ["value1", "value2"]);
        yield return () => ([new SimpleRecord("value", 1), new SimpleRecord("value2", 2)], [new SimpleRecord("value", 1), new SimpleRecord("value2", 2)]);
    }

    public static IEnumerable<Func<(object[], object[])>> GetNotEqualEnumerableTypes()
    {
        yield return () => ([1, 2, 3], [1, 2, 4]);
        yield return () => (["value1", "value2"], ["value1", "value3"]);
        yield return () => ([new SimpleRecord("value", 1), new SimpleRecord("value2", 2)], [new SimpleRecord("value", 1), new SimpleRecord("value2", 3)]);
    }

    private record SimpleRecord(string Value, int IntValue);
}
