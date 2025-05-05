using FastEndpoints;
using Microsoft.Extensions.Primitives;
using WebApp.Common.IdEncoding;

namespace WebApp.Api.V1.Common.Converters;

public static class EncodedEntityIdValueParsers
{
    public static ParseResult ParseLong<T>(IIdEncoder idEncoder, StringValues input, Func<long, T> outputFn)
    {
        var first = input.FirstOrDefault();
        if (first is null)
        {
            return new ParseResult(false, default);
        }

        if (!idEncoder.TryDecode(first, out var id))
        {
            return new ParseResult(false, default);
        }
        return new ParseResult(true, outputFn(id));
    }
}
