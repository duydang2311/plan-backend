using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseTeamIssueDto
{
    public Instant? CreatedTime { get; init; }
    public TeamId? TeamId { get; init; }
    public BaseTeamDto? Team { get; init; }
    public IssueId? IssueId { get; init; }
    public BaseIssueDto? Issue { get; init; }
    public string? Rank { get; init; }
}
