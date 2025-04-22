namespace WebApp.Domain.Entities;

public record ProjectResource
{
    public ResourceId ResourceId { get; init; }
    public Resource Resource { get; init; } = null!;
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
}
