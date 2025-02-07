namespace WebApp.Api.V1.Common.Helpers;

using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;

public static class ProblemExtensions
{
    public static Problem Failure(this Problem problem, string name, string message)
    {
        problem.Failures.Add(new ValidationFailure(name, message));
        return problem;
    }

    public static Problem Failure(this Problem problem, string name, string message, string code)
    {
        problem.Failures.Add(new ValidationFailure(name, message) { ErrorCode = code });
        return problem;
    }

    public static Conflict<ProblemDetails> ToConflict(this ProblemDetails problemDetails)
    {
        return TypedResults.Conflict(problemDetails);
    }
}
