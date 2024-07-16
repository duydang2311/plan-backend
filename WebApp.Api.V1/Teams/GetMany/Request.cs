using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Teams.GetMany;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetMany;

public sealed record Request : Collective
{
    public WorkspaceId? WorkspaceId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetTeams ToCommand(this Request request);
}
