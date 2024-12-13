using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceInvitations.Create;

public sealed record CreateWorkspaceInvitation : ICommand<OneOf<ValidationFailures, Success>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required UserId UserId { get; init; }
}
