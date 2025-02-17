namespace Shouldst.Tests;

public class ExceptionTests
{
    [Test]
    public void AssertExceptionContainsMessageWithMatchingSucceeds()
    {
        new Exception("error message").ShouldContainErrorMessage(" message");
    }

    [Test]
    public void AssertExceptionContainsMessageWithMissingThrows()
    {
        Assert.Throws<ShouldException>(() => new Exception("error message").ShouldContainErrorMessage("missing"));
    }
}
