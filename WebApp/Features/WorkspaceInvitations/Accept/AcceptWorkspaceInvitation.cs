using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceInvitations.Accept;

public sealed record AcceptWorkspaceInvitation : ICommand<OneOf<NotFoundError, ConflictError, Success>>
{
    public required WorkspaceInvitationId Id { get; init; }
}
