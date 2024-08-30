using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Converters;

public sealed class NullableEntityIdJsonConverter<TId, TValue>
    where TId : struct, IEntityId<TValue>
{
    public static ParseResult ValueParser(object? x)
    {
        throw new NotImplementedException();
    }
}
