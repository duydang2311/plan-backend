using System.Security.Claims;
using FastEndpoints;

namespace WebApp.Api.V1.Workspaces.Get.ByPath;

public sealed record Request
{
    public string Path { get; init; } = string.Empty;

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public Guid UserId { get; init; }
}
