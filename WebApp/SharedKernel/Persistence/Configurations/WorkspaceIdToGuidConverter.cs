using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence.Configurations;

public sealed class WorkspaceIdToGuidConverter : ValueConverter<WorkspaceId, Guid>
{
    public WorkspaceIdToGuidConverter()
        : base(value => value.Value, (value) => new WorkspaceId(value)) { }
}
