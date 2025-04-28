using FastEndpoints;
using Microsoft.Extensions.Primitives;
using Sqids;

namespace WebApp.Api.V1.Common.Converters;

public static class EncodedEntityIdValueParsers
{
    public static ParseResult ParseLong<T>(SqidsEncoder<long> encoder, StringValues input, Func<long, T> outputFn)
    {
        var first = input.FirstOrDefault();
        if (first is null)
        {
            return new ParseResult(false, default);
        }
        var ids = encoder.Decode(first);
        if (ids.Count != 1)
        {
            return new ParseResult(false, default);
        }
        return new ParseResult(true, outputFn(ids[0]));
    }
}
