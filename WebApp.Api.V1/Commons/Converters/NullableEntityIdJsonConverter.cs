using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Commons.Converters;

public sealed class NullableEntityIdJsonConverter<T>
    where T : struct, IEntityId
{
    public static ParseResult ValueParser(object? x)
    {
        var result = EntityIdJsonConverter<T>.ValueParser(x);
        return result.IsSuccess
            ? result
            : new ParseResult(true, null);
    }
}
