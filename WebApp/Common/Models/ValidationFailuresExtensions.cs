using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Common.Models;

public static class ValidationFailuresExtensions
{
    public static ProblemDetails ToProblemDetails(
        this ValidationFailures validationFailures,
        string? instance = null,
        string? traceId = null,
        int? statusCode = null
    )
    {
        return new ProblemDetails(validationFailures.Failures, instance!, traceId!, statusCode ?? 400);
    }

    public static UnprocessableEntity<ProblemDetails> ToUnprocessableEntity(
        this ValidationFailures validationFailures,
        string? instance = null,
        string? traceId = null
    )
    {
        return TypedResults.UnprocessableEntity(
            validationFailures.ToProblemDetails(instance: instance, traceId: traceId, statusCode: 422)
        );
    }

    public static ValidationFailures ToValidationFailures(
        this ReferenceConstraintException referenceConstraintException,
        Func<string, (string PropertyName, string Message)?> transform
    )
    {
        var failures = ValidationFailures.Many(referenceConstraintException.ConstraintProperties.Count);
        foreach (var property in referenceConstraintException.ConstraintProperties)
        {
            var tuple = transform(property);
            if (tuple.HasValue)
            {
                failures.Add(tuple.Value.PropertyName, tuple.Value.Message, "invalid_reference");
            }
            else
            {
                failures.Add(property, $"Reference to \"{property}\" does not exist", "invalid_reference");
            }
        }
        return failures;
    }

    public static ValidationFailures ToValidationFailures(
        this UniqueConstraintException uniqueConstraintException,
        Func<string, (string PropertyName, string Message)?> transform
    )
    {
        var failures = ValidationFailures.Many(uniqueConstraintException.ConstraintProperties.Count);
        foreach (var property in uniqueConstraintException.ConstraintProperties)
        {
            var tuple = transform(property);
            if (tuple.HasValue)
            {
                failures.Add(tuple.Value.PropertyName, tuple.Value.Message, "unique");
            }
            else
            {
                failures.Add(property, $"\"{property}\" must be unique", "unique");
            }
        }
        return failures;
    }
}
