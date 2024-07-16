using FluentValidation.Results;

namespace FastEndpoints;

public static class ValidationExtensions
{
    public static ProblemDetails ToProblemDetails(
        this IEnumerable<ValidationFailure> validationFailures,
        int? statusCode = null
    ) => ToProblemDetails([.. validationFailures], statusCode);

    public static ProblemDetails ToProblemDetails(
        this IReadOnlyList<ValidationFailure> validationFailures,
        int? statusCode = null
    )
    {
        return new ProblemDetails(validationFailures, statusCode);
    }
}
