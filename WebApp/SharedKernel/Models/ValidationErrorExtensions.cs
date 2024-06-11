using FluentValidation.Results;
using WebApp.SharedKernel.Models;

namespace FastEndpoints;

public static class ValidationErrorExtensions
{
    public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationError> errors, int? statusCode = null)
    {
        return new ProblemDetails(
            [.. errors.Select(a => new ValidationFailure(a.Name, a.Reason) { ErrorCode = a.ErrorCode })],
            statusCode);
    }

    public static ProblemDetails ToProblemDetails(this ValidationError error, int? statusCode = null)
    {
        return new ProblemDetails(
            [new ValidationFailure(error.Name, error.Reason) { ErrorCode = error.ErrorCode }],
            statusCode);
    }
}
