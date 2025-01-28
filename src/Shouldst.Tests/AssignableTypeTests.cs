namespace Shouldst.Tests;

public class AssignableTypeTests
{
    [Test]
    public void AssertAssignableTypeWithSameSucceeds()
    {
        var value = new MyType();

        value.ShouldBeAssignableTo<MyType>();
        value.ShouldBeAssignableTo(typeof(MyType));
    }

    [Test]
    public void AssertNotAssignableTypeWithSameThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo(typeof(MyType)));
    }

    [Test]
    public void AssertAssignableTypeWithDifferentThrows()
    {
        var value = new MyType();

        Assert.Throws<ShouldException>(() => value.ShouldBeAssignableTo<IsolatedType>());
        Assert.Throws<ShouldException>(() => value.ShouldBeAssignableTo(typeof(IsolatedType)));
    }

    [Test]
    public void AssertNotAssignableTypeWithDifferentSucceeds()
    {
        var value = new MyType();

        value.ShouldNotBeAssignableTo<IsolatedType>();
        value.ShouldNotBeAssignableTo(typeof(IsolatedType));
    }

    [Test]
    public void AssertAssignableTypeWithDerivedSucceeds()
    {
        var value = new MyDerivedType();

        value.ShouldBeAssignableTo<MyType>();
        value.ShouldBeAssignableTo(typeof(MyType));
    }

    [Test]
    public void AssertAssignableTypeWithImplementedSucceeds()
    {
        var value = new MyDerivedType();

        value.ShouldBeAssignableTo<IMyInterface>();
        value.ShouldBeAssignableTo(typeof(IMyInterface));
    }

    [Test]
    public void AssertNotAssignableTypeWithDerivedThrows()
    {
        var value = new MyDerivedType();

        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo<MyType>());
        Assert.Throws<ShouldException>(() => value.ShouldNotBeAssignableTo(typeof(MyType)));
    }

    [Test]
    public void AssertNotAssignableTypeWithImplementedThrows()
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
