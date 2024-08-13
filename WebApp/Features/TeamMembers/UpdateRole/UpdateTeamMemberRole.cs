using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamMembers.UpdateRole;

public sealed record UpdateTeamMemberRole : ICommand<OneOf<ValidationFailures, Success>>
{
    public required TeamId TeamId { get; init; }
    public required UserId MemberId { get; init; }
    public TeamRoleId? TeamRoleId { get; init; }
    public string? RoleName { get; init; }
}
