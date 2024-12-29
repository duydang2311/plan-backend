using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceMembers.Delete;

namespace WebApp.Api.V1.WorkspaceMembers.Delete;

public sealed record Request
{
    public WorkspaceMemberId Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.UserId))]
    public static partial DeleteWorkspaceMember ToCommand(this Request request);
}
