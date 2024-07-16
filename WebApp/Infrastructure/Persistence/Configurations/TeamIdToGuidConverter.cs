using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence.Configurations;

public sealed class TeamIdToGuidConverter : ValueConverter<TeamId, Guid>
{
    public TeamIdToGuidConverter()
        : base(value => value.Value, (value) => new TeamId(value)) { }
}
