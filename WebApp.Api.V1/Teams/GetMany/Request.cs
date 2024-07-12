using System.Security.Claims;
using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetMany;

public sealed record Request
{
    public WorkspaceId? WorkspaceId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}
