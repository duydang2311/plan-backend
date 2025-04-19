using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.GetPermissions;

using Result = PaginatedList<string>;

public sealed class GetWorkspaceMemberPermissionsHandler(AppDbContext db, IPermissionCache permissionCache)
    : ICommandHandler<GetWorkspaceMemberPermissions, Result>
{
    public async Task<Result> ExecuteAsync(GetWorkspaceMemberPermissions command, CancellationToken ct)
    {
        var query = db.WorkspaceMembers.AsQueryable();

        var workspaceId = command.WorkspaceId;
        var userId = command.UserId;
        if (command.Id.HasValue)
        {
            var workspaceMember = await db
                .WorkspaceMembers.Where(a => a.Id == command.Id.Value)
                .Select(a => new { a.WorkspaceId, a.UserId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

            if (workspaceMember is not null)
            {
                workspaceId = workspaceMember.WorkspaceId;
                userId = workspaceMember.UserId;
            }
        }

        if (workspaceId.HasValue && userId.HasValue)
        {
            var permissions = await permissionCache
                .GetWorkspacePermissionsAsync(workspaceId.Value, userId.Value, ct)
                .ConfigureAwait(false);
            return PaginatedList.From(permissions, permissions.Count);
        }

        return PaginatedList.From<string>([], 0);
    }
}
