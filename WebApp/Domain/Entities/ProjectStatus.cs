namespace WebApp.Domain.Entities;

public sealed record class ProjectStatus
{
    public ProjectId ProjectId { get; init; } = ProjectId.Empty;
    public Project Project { get; init; } = null!;
    public StatusId StatusId { get; init; } = StatusId.Empty;
    public Status Status { get; init; } = null!;
}
