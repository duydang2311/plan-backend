using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;

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
                failures.Add(tuple.Value.PropertyName, tuple.Value.Message, ErrorCodes.NotFound);
            }
            else
            {
                failures.Add(property, $"Reference to \"{property}\" does not exist", ErrorCodes.NotFound);
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
                failures.Add(tuple.Value.PropertyName, tuple.Value.Message, ErrorCodes.Duplicated);
            }
            else
            {
                failures.Add(property, $"\"{property}\" must be unique", ErrorCodes.Duplicated);
            }
        }
        return failures;
    }
}
