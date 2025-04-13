using System.Diagnostics.CodeAnalysis;

namespace WebApp.Common.Helpers;

public sealed record Attempt<TData, TError>
{
    public TData? Data { get; init; }
    public TError? Error { get; init; }
}

public static class AttemptExtensions
{
    public static bool TryGetData<TData, TError>(
        this Attempt<TData, TError> attempt,
        [NotNullWhen(true)] out TData? data,
        [NotNullWhen(false)] out TError? error
    )
    {
        if (attempt.Error is null)
        {
            error = default;
            data = attempt.Data!;
            return true;
        }

        error = attempt.Error;
        data = default;
        return false;
    }

    public static bool TryGetError<TData, TError>(
        this Attempt<TData, TError> attempt,
        [NotNullWhen(true)] out TError? error,
        [NotNullWhen(false)] out TData? data
    )
    {
        if (attempt.Error is null)
        {
            error = default;
            data = attempt.Data!;
            return false;
        }

        error = attempt.Error;
        data = default;
        return true;
    }
}

public static class AttemptHelper
{
    public static Attempt<T, Exception> Attempt<T>(Func<T> action)
    {
        try
        {
            return new Attempt<T, Exception> { Data = action() };
        }
        catch (Exception e)
        {
            return new Attempt<T, Exception> { Error = e };
        }
    }

    public static Attempt<T, TError> Attempt<T, TError>(Func<T> action, Func<Exception, TError> mapException)
    {
        try
        {
            return new Attempt<T, TError> { Data = action() };
        }
        catch (Exception e)
        {
            return new Attempt<T, TError> { Error = mapException(e) };
        }
    }

    public static async Task<Attempt<T, Exception>> AttemptAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return new Attempt<T, Exception> { Data = await action().ConfigureAwait(false) };
        }
        catch (Exception e)
        {
            return new Attempt<T, Exception> { Error = e };
        }
    }

    public static async Task<Attempt<T, TError>> AttemptAsync<T, TError>(
        Func<Task<T>> action,
        Func<Exception, TError> mapException
    )
    {
        try
        {
            return new Attempt<T, TError> { Data = await action().ConfigureAwait(false) };
        }
        catch (Exception e)
        {
            return new Attempt<T, TError> { Error = mapException(e) };
        }
    }
}
