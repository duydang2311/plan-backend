using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseMilestoneDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public MilestoneId? Id { get; init; }
    public ProjectId? ProjectId { get; init; }
    public Project? Project { get; init; }
    public Instant? EndTime { get; init; }
    public string? Title { get; init; }
    public string? Emoji { get; init; }
    public string? Color { get; init; }
    public string? Description { get; init; }
    public MilestoneStatusId? StatusId { get; init; }
    public BaseMilestoneStatusDto? Status { get; init; }
}
