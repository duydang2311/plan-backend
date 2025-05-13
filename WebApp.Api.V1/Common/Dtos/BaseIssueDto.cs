using NodaTime;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseIssueDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public IssueId? Id { get; init; }
    public UserId? AuthorId { get; init; }
    public BaseUserDto? Author { get; init; }
    public ProjectId? ProjectId { get; init; }
    public Project? Project { get; init; }
    public long? OrderNumber { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? PreviewDescription { get; init; }
    public StatusId? StatusId { get; init; }
    public Status? Status { get; init; }
    public string? StatusRank { get; init; }
    public Instant? StartTime { get; init; }
    public Instant? EndTime { get; init; }
    public string? TimelineZone { get; init; }
    public IssuePriority? Priority { get; init; }
    public ICollection<BaseTeamDto>? Teams { get; init; }
    public ICollection<BaseUserDto>? Assignees { get; init; }
}
