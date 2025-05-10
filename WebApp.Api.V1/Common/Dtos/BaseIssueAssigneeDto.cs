using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseIssueAssigneeDto
{
    public Instant? CreatedTime { get; init; }
    public IssueId? IssueId { get; init; }
    public BaseIssueDto? Issue { get; init; }
    public UserId? UserId { get; init; }
    public BaseUserDto? User { get; init; }
}
