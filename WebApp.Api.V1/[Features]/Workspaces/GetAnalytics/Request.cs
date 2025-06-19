using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Workspaces.GetAnalytics;

namespace WebApp.Api.V1.Workspaces.GetAnalytics;

public sealed record Request
{
    public required WorkspaceId WorkspaceId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetWorkspaceAnalytics ToCommand(this Request request);
}
