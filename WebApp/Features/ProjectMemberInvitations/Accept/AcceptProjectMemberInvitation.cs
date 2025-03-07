using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMemberInvitations.Accept;

public sealed record AcceptProjectMemberInvitation : ICommand<OneOf<NotFoundError, Success>>
{
    public required ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
}
