using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.TeamMembers.GetMany;

namespace WebApp.Api.V1.TeamMembers.GetMany;

public sealed record Request : Collective
{
    public TeamId TeamId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetTeamMembers ToCommand(this Request request);
}
