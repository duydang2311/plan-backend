using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.GetPermissions;

public sealed record GetProjectMemberPermissions : ICommand<PaginatedList<string>>
{
    public ProjectMemberId? Id { get; init; }
    public ProjectId? ProjectId { get; init; }
    public UserId? UserId { get; init; }
}
