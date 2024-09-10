using FastEndpoints;
using FluentValidation.Results;

namespace WebApp.Common.Models;

public sealed class ValidationFailures
{
    public List<ValidationFailure> Failures { get; init; } = [];

    private ValidationFailures() { }

    public static ValidationFailures Many(int capacity)
    {
        return new ValidationFailures { Failures = new(capacity) };
    }

    public static ValidationFailures Single(string name, string message)
    {
        return new ValidationFailures { Failures = [new ValidationFailure(name, message)] };
    }

    public static ValidationFailures Single(string name, string message, string code)
    {
        return new ValidationFailures { Failures = [new ValidationFailure(name, message) { ErrorCode = code }] };
    }

    public ValidationFailures Add(string name, string message)
    {
        Failures.Add(new ValidationFailure(name, message));
        return this;
    }

    public ValidationFailures Add(string name, string message, string code)
    {
        Failures.Add(new ValidationFailure(name, message) { ErrorCode = code });
        return this;
    }

    public ProblemDetails ToProblemDetails(string? instance = null, string? traceId = null, int? statusCode = null)
    {
        return new ProblemDetails(Failures, instance!, traceId!, statusCode ?? 400);
    }
}
