using System.Security.Claims;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Teams.GetOne.ByIdentifier;

public sealed record Request
{
    public WorkspaceId WorkspaceId { get; init; }
    public string Identifier { get; init; } = string.Empty;

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}
