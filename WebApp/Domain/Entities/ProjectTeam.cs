namespace WebApp.Domain.Entities;

public sealed record ProjectTeam
{
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
}
