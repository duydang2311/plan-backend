using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class EntityGuidConverter<T> : ValueConverter<T, Guid>
    where T : struct, IEntityGuid
{
    public EntityGuidConverter()
        : base(value => value.Value, (value) => new T { Value = value }) { }
}
