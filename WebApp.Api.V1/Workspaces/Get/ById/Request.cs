using System.Security.Claims;
using FastEndpoints;

namespace WebApp.Api.V1.Workspaces.Get.ById;

public sealed record Request
{
    public Guid WorkspaceId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public Guid UserId { get; init; }
}
