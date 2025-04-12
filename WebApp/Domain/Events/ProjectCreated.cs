using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ProjectCreated
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required ProjectId ProjectId { get; init; }
}
