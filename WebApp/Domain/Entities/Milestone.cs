using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record Milestone
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public MilestoneId Id { get; init; }
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public Instant EndTime { get; init; }
    public string Title { get; init; } = null!;
    public string Emoji { get; init; } = null!;
    public string Color { get; init; } = null!;
    public string? Description { get; init; }
    public string? PreviewDescription { get; init; }
    public MilestoneStatusId? StatusId { get; init; }
    public MilestoneStatus? Status { get; init; }
    public ICollection<Issue> Issues { get; init; } = null!;
}
