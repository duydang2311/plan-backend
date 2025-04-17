using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceInvitations.GetOne;

public sealed record GetWorkspaceInvitation : ICommand<OneOf<NotFoundError, WorkspaceInvitation>>
{
    public required WorkspaceInvitationId Id { get; init; }
    public string? Select { get; init; }
}
