using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Commons.Converters;

public sealed class NullableEntityGuidJsonConverter<T>
    where T : struct, IEntityGuid
{
    public static ParseResult ValueParser(object? x)
    {
        var result = EntityGuidJsonConverter<T>.ValueParser(x);
        return result.IsSuccess ? result : new ParseResult(true, null);
    }
}
