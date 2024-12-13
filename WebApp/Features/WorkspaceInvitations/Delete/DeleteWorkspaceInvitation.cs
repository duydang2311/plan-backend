using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceInvitations.Delete;

public sealed record DeleteWorkspaceInvitation : ICommand<OneOf<NotFoundError, Success>>
{
    public required WorkspaceInvitationId Id { get; init; }
}
