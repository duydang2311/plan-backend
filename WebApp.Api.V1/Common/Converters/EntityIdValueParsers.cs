using System.Globalization;
using FastEndpoints;

namespace WebApp.Api.V1.Common.Converters;

public static class EntityIdValueParsers
{
    public static ParseResult ParseLong<T>(object? input, Func<long, T> outputFn)
    {
        if (long.TryParse(input?.ToString(), CultureInfo.InvariantCulture, out var value))
        {
            return new ParseResult(true, outputFn(value));
        }
        return new ParseResult(false, default);
    }
}
