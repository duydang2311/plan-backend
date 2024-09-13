using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceMembers.Get;

namespace WebApp.Api.V1.WorkspaceMembers.Get;

public sealed record Request : Collective
{
    public WorkspaceId WorkspaceId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetWorkspaceMembers ToCommand(this Request request);
}
