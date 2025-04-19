using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.GetPermissions;

using Result = PaginatedList<string>;

public sealed class GetProjectMemberPermissionsHandler(AppDbContext db, IPermissionCache permissionCache)
    : ICommandHandler<GetProjectMemberPermissions, Result>
{
    public async Task<Result> ExecuteAsync(GetProjectMemberPermissions command, CancellationToken ct)
    {
        var query = db.ProjectMembers.AsQueryable();

        var projectId = command.ProjectId;
        var userId = command.UserId;
        if (command.Id.HasValue)
        {
            var projectMember = await db
                .ProjectMembers.Where(a => a.Id == command.Id.Value)
                .Select(a => new { a.ProjectId, a.UserId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);

            if (projectMember is not null)
            {
                projectId = projectMember.ProjectId;
                userId = projectMember.UserId;
            }
        }

        if (projectId.HasValue && userId.HasValue)
        {
            var permissions = await permissionCache
                .GetProjectPermissionsAsync(projectId.Value, userId.Value, ct)
                .ConfigureAwait(false);
            return PaginatedList.From(permissions, permissions.Count);
        }

        return PaginatedList.From<string>([], 0);
    }
}
