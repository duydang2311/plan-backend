namespace WebApp.Domain.Entities;

public sealed record ProjectMember : UserRole
{
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
}
