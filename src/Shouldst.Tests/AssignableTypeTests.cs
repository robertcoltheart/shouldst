namespace Shouldst.Tests;

public class AssignableTypeTests
{
    [Test]
    public void ShouldBeAssignableToTypeWithSameTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldBeAssignableTo<MyType>();
        value.ShouldBeAssignableTo(typeof(MyType));
    }

    [Test]
    public void ShouldNotBeAssignableToTypeWithSameTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo(typeof(MyType)));
    }

    [Test]
    public void ShouldBeAssignableToTypeWithDifferentTypeThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeAssignableTo<IsolatedType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeAssignableTo(typeof(IsolatedType)));
    }

    [Test]
    public void ShouldNotBeAssignableToTypeWithDifferentTypeSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeAssignableTo<IsolatedType>();
        value.ShouldNotBeAssignableTo(typeof(IsolatedType));
    }

    [Test]
    public void ShouldBeAssignableToTypeWithDerivedTypeSucceeds()
    {
        var value = new MyDerivedType();

        value.ShouldBeAssignableTo<MyType>();
        value.ShouldBeAssignableTo(typeof(MyType));
    }

    [Test]
    public void ShouldBeAssignableToTypeWithImplementedTypeSucceeds()
    {
        var value = new MyDerivedType();

        value.ShouldBeAssignableTo<IMyInterface>();
        value.ShouldBeAssignableTo(typeof(IMyInterface));
    }

    [Test]
    public void ShouldNotBeAssignableToTypeWithDerivedTypeThrows()
    {
        var value = new MyDerivedType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo(typeof(MyType)));
    }

    [Test]
    public void ShouldNotBeAssignableToTypeWithImplementedTypeThrows()
    {
        var value = new MyDerivedType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo<IMyInterface>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo(typeof(IMyInterface)));
    }

    private class MyType;

    private class MyDerivedType : MyType, IMyInterface;

    private class IsolatedType;

    private interface IMyInterface;
}
