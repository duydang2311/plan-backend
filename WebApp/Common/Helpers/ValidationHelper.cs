using FluentValidation.Results;

namespace WebApp.Common.Helpers;

public static class ValidationHelper
{
    public static ValidationFailure Fail(string name, string reason) => Fail(name, reason, string.Empty);

    public static ValidationFailure Fail(string name, string reason, string code)
    {
        return new ValidationFailure(name, reason) { ErrorCode = code };
    }
}
