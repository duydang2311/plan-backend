using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMemberInvitations.Delete;

public sealed record DeleteProjectMemberInvitation : ICommand<OneOf<NotFoundError, Success>>
{
    public required ProjectMemberInvitationId Id { get; init; }
}
