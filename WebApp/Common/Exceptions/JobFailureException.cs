namespace WebApp.Common.Exceptions;

public sealed class JobFailureException : Exception
{
    public JobFailureException() { }

    public JobFailureException(string message)
        : base(message) { }

    public JobFailureException(string message, Exception inner)
        : base(message, inner) { }
}
