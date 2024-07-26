using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class EntityIdConverter<TId, TValue> : ValueConverter<TId, TValue>
    where TId : struct, IEntityId<TValue>
{
    public EntityIdConverter()
        : base(value => value.Value, (value) => new TId { Value = value }) { }
}
