using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.Patch;

public sealed class PatchWorkspaceMemberHandler(AppDbContext db, IPermissionCache permissionCache)
    : ICommandHandler<PatchWorkspaceMember, OneOf<NotFoundError, ForbiddenError, InvalidPatchError, Success>>
{
    public async Task<OneOf<NotFoundError, ForbiddenError, InvalidPatchError, Success>> ExecuteAsync(
        PatchWorkspaceMember command,
        CancellationToken ct
    )
    {
        var workspaceMember = await db
            .WorkspaceMembers.Where(a => a.Id == command.Id)
            .Select(a => new
            {
                a.WorkspaceId,
                a.UserId,
                a.Role.Rank,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (workspaceMember is null)
        {
            return new NotFoundError();
        }

        var requestingWorkspaceMember = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == workspaceMember.WorkspaceId && a.UserId == command.RequestingUserId
            )
            .Select(a => new { a.Role.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (requestingWorkspaceMember is null)
        {
            return new ForbiddenError();
        }

        var requireCacheInvalidation = false;
        Expression<Func<SetPropertyCalls<WorkspaceMember>, SetPropertyCalls<WorkspaceMember>>>? expression = default;
        if (command.Patch.TryGetValue(a => a.RoleId, out var roleId))
        {
            var role = WorkspaceRoleDefaults.Roles.FirstOrDefault(a => a.Id == roleId);
            if (role is null || requestingWorkspaceMember.Rank >= role.Rank)
            {
                return new ForbiddenError();
            }

            expression = ExpressionHelper.Append(expression, a => a.SetProperty(b => b.RoleId, roleId));
            requireCacheInvalidation = true;
        }
        if (expression is null)
        {
            return new InvalidPatchError();
        }

        var count = await db
            .WorkspaceMembers.Where(a => a.Id == command.Id)
            .ExecuteUpdateAsync(expression, ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }

        if (requireCacheInvalidation)
        {
            await permissionCache
                .InvalidateWorkspacePermissionAsync(workspaceMember.WorkspaceId, workspaceMember.UserId, ct)
                .ConfigureAwait(false);
        }
        return new Success();
    }
}
