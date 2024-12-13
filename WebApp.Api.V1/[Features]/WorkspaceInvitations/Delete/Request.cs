using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceInvitations.Delete;

namespace WebApp.Api.V1.WorkspaceInvitations.Delete;

public sealed record Request
{
    public WorkspaceInvitationId Id { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial DeleteWorkspaceInvitation ToCommand(this Request request);
}
