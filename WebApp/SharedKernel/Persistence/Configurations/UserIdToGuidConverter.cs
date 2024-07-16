using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class UserIdToGuidConverter : ValueConverter<UserId, Guid>
{
    public UserIdToGuidConverter()
        : base(value => value.Value, (value) => new UserId(value)) { }
}
