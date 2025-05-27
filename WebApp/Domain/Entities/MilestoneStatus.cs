using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record MilestoneStatus
{
    public MilestoneStatusId Id { get; init; }
    public MilestoneStatusCategory Category { get; init; }
    public int Rank { get; init; }
    public string Value { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string? Icon { get; init; }
    public string? Description { get; init; }
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public bool IsDefault { get; init; }

    public ICollection<Milestone> Milestones { get; init; } = null!;
}
