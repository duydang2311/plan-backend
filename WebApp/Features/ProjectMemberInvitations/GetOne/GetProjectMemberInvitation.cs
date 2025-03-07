using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMemberInvitations.GetOne;

public sealed record GetProjectMemberInvitation : ICommand<OneOf<NotFoundError, ProjectMemberInvitation>>
{
    public required ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
    public string? Select { get; init; }
}
