using System.Security.Claims;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.Get.ById;

public sealed record Request
{
    public WorkspaceId WorkspaceId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}
