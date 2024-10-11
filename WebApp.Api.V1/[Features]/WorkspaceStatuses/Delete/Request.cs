using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceStatuses.Delete;

namespace WebApp.Api.V1.WorkspaceStatuses.Delete;

public sealed record Request
{
    public StatusId StatusId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial DeleteWorkspaceStatus ToCommand(this Request request);
}
