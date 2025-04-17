using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseWorkspaceInvitationDto
{
    public Instant? CreatedTime { get; init; }
    public WorkspaceInvitationId? Id { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public BaseWorkspaceDto? Workspace { get; init; }
    public UserId? UserId { get; init; }
    public BaseUserDto? User { get; init; }
}
