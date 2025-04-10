using System.Security.Claims;
using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Hubs.GetToken;

public sealed record Request
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}
