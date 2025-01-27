namespace Shouldst.Tests;

public class ExactTypeTests
{
    [Test]
    public void AssertExactTypeWithExactTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldBeOfExactType<MyType>();
        value.ShouldBeOfExactType(typeof(MyType));
    }

    [Test]
    public void AssertExactTypeWithDifferentTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<IsolatedType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(IsolatedType)));
    }

    [Test]
    public void AssertExactTypeWithDerivedTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<DerivedMyType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(DerivedMyType)));
    }

    [Test]
    public void AssertExactTypeWithNullThrows()
    {
        var value = default(MyType);

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(MyType)));
    }

    [Test]
    public void AssertNotExactTypeWithExactTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType(typeof(MyType)));
    }

    [Test]
    public void AssertNotExactTypeWithDifferentTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeOfExactType<IsolatedType>();
        value.ShouldNotBeOfExactType(typeof(IsolatedType));
    }

    [Test]
    public void AssertNotExactTypeWithDerivedTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeOfExactType<DerivedMyType>();
        value.ShouldNotBeOfExactType(typeof(DerivedMyType));
    }

    [Test]
    public void AssertNotExactTypeWithNullThrows()
    {
        var value = default(MyType);

        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType(typeof(MyType)));
    }

    private class MyType;

    private class DerivedMyType : MyType;

    private class IsolatedType;
}
