using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.Get;

public sealed record Response
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public ICollection<Status> Statuses { get; init; } = null!;
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this Workspace workspace);
}
