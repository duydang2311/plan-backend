namespace WebApp.Domain.Entities;

public sealed record class WorkspaceStatus
{
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public Workspace Workspace { get; init; } = null!;
    public StatusId StatusId { get; init; } = StatusId.Empty;
    public Status Status { get; init; } = null!;
}
