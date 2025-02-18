namespace Shouldst.Tests;

public class ActionTests
{
    [Test]
    public void ShouldThrowWithThrowingMatchingExceptionSucceeds()
    {
        var action = ThrowException;

        action.ShouldThrow<Exception>();
        action.ShouldThrow(typeof(Exception));
    }

    [Test]
    public void ShouldThrowWithThrowingMissingExceptionThrows()
    {
        var action = ThrowException;

        Assert.Throws<ShouldException>(() => action.ShouldThrow<InvalidOperationException>());
        Assert.Throws<ShouldException>(() => action.ShouldThrow(typeof(InvalidOperationException)));
    }

    [Test]
    public void ShouldThrowWithNoExceptionThrows()
    {
        var action = DoNotThrow;

        Assert.Throws<ShouldException>(() => action.ShouldThrow<Exception>());
        Assert.Throws<ShouldException>(() => action.ShouldThrow(typeof(Exception)));
    }

    private void ThrowException()
    {
        throw new Exception();
    }

    private void DoNotThrow()
    {
    }
}
