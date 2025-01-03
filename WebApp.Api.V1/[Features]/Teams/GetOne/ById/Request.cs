using System.Security.Claims;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Teams.GetOne.ById;

public sealed record Request
{
    public TeamId Id { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}
