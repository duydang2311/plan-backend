using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class EntityIdToGuidConverter<T> : ValueConverter<T, Guid>
    where T : struct, IEntityId
{
    public EntityIdToGuidConverter()
        : base(value => value.Value, (value) => new T { Value = value }) { }
}
