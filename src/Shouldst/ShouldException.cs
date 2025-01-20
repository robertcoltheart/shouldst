namespace Shouldst;

public class ShouldException : Exception
{
    public ShouldException(string message)
        : base(message)
    {
    }

    public ShouldException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
