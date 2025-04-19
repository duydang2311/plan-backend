using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceMembers.GetPermissions;

public sealed record GetWorkspaceMemberPermissions : ICommand<PaginatedList<string>>
{
    public WorkspaceMemberId? Id { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public UserId? UserId { get; init; }
}
