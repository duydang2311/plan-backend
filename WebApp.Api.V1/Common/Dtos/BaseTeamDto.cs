using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseTeamDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public TeamId? Id { get; init; }
    public string? Name { get; init; }
    public string? Identifier { get; init; }
    public ICollection<BaseUserDto>? Members { get; set; }
}
