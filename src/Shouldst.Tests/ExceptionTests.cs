namespace Shouldst.Tests;

public class ExceptionTests
{
    [Test]
    public void ShouldContainExceptionMessageWithMatchingSucceeds()
    {
        new Exception("error message").ShouldContainErrorMessage(" message");
    }

    [Test]
    public void ShouldContainExceptionMessageWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => new Exception("error message").ShouldContainErrorMessage("missing"));
    }
}
