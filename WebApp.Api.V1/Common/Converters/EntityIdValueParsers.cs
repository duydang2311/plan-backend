using System.Globalization;
using FastEndpoints;
using Microsoft.Extensions.Primitives;

namespace WebApp.Api.V1.Common.Converters;

public static class EntityIdValueParsers
{
    public static ParseResult ParseLong<T>(StringValues input, Func<long, T> outputFn)
    {
        var first = input.FirstOrDefault();
        if (first is null)
        {
            return new ParseResult(false, default);
        }
        if (long.TryParse(first, CultureInfo.InvariantCulture, out var value))
        {
            return new ParseResult(true, outputFn(value));
        }
        return new ParseResult(false, default);
    }

    public static ParseResult ParseInt<T>(StringValues input, Func<int, T> outputFn)
    {
        var first = input.FirstOrDefault();
        if (first is null)
        {
            return new ParseResult(false, default);
        }
        if (int.TryParse(first, CultureInfo.InvariantCulture, out var value))
        {
            return new ParseResult(true, outputFn(value));
        }
        return new ParseResult(false, default);
    }

    public static ParseResult ParseString<T>(StringValues input, Func<string, T> outputFn)
    {
        var value = input.FirstOrDefault();
        if (value is not null)
        {
            return new ParseResult(true, outputFn(value));
        }
        return new ParseResult(false, default);
    }
}
