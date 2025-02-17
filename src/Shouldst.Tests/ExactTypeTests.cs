namespace Shouldst.Tests;

public class ExactTypeTests
{
    [Test]
    public void ShouldBeOfExactTypeWithExactTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldBeOfExactType<MyType>();
        value.ShouldBeOfExactType(typeof(MyType));
    }

    [Test]
    public void ShouldBeOfExactTypeWithDifferentTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<IsolatedType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(IsolatedType)));
    }

    [Test]
    public void ShouldBeOfExactTypeWithDerivedTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<DerivedMyType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(DerivedMyType)));
    }

    [Test]
    public void ShouldBeOfExactTypeWithNullThrows()
    {
        var value = default(MyType);

        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeOfExactType(typeof(MyType)));
    }

    [Test]
    public void ShouldNotBeOfExactTypeWithExactTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType(typeof(MyType)));
    }

    [Test]
    public void ShouldNotBeOfExactTypeWithDifferentTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeOfExactType<IsolatedType>();
        value.ShouldNotBeOfExactType(typeof(IsolatedType));
    }

    [Test]
    public void AssertNoShouldNotBeOfExactTypeWithDerivedTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeOfExactType<DerivedMyType>();
        value.ShouldNotBeOfExactType(typeof(DerivedMyType));
    }

    [Test]
    public void ShouldNotBeOfExactTypeWithNullThrows()
    {
        var value = default(MyType);

        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeOfExactType(typeof(MyType)));
    }

    private class MyType;

    private class DerivedMyType : MyType;

    private class IsolatedType;
}
