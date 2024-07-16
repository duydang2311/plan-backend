using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class WorkspaceIdToGuidConverter : ValueConverter<WorkspaceId, Guid>
{
    public WorkspaceIdToGuidConverter()
        : base(value => value.Value, (value) => new WorkspaceId(value)) { }
}
