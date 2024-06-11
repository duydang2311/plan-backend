namespace WebApp.SharedKernel.Models;

public sealed record class ValidationError(string Name, string Reason)
{
    public string ErrorCode { get; init; } = string.Empty;

    public ValidationError(string name, string reason, string errorCode) : this(name, reason)
    {
        this.ErrorCode = errorCode;
    }
}
