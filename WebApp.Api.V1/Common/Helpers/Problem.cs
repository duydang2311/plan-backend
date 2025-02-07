using FastEndpoints;
using FluentValidation.Results;

namespace WebApp.Api.V1.Common.Helpers;

public sealed class Problem
{
    public List<ValidationFailure> Failures { get; init; } = [];
    string? instance;
    string? traceId;
    int? statusCode;

    private Problem() { }

    public static Problem Multiple(int capacity)
    {
        return new Problem { Failures = new(capacity) };
    }

    public static Problem Failure(string name, string message)
    {
        return new Problem { Failures = [new ValidationFailure(name, message)] };
    }

    public static Problem Failure(string name, string message, string code)
    {
        return new Problem { Failures = [new ValidationFailure(name, message) { ErrorCode = code }] };
    }

    public Problem Instance(string? instance)
    {
        this.instance = instance;
        return this;
    }

    public Problem TraceId(string? traceId)
    {
        this.traceId = traceId;
        return this;
    }

    public Problem StatusCode(int? statusCode)
    {
        this.statusCode = statusCode;
        return this;
    }

    public ProblemDetails ToProblemDetails(string? instance = null, string? traceId = null, int? statusCode = null)
    {
        return new ProblemDetails(
            Failures,
            instance ?? this.instance!,
            traceId ?? this.traceId!,
            statusCode ?? this.statusCode ?? 400
        );
    }
}
