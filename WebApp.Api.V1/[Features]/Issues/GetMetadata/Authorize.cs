using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetMetadata;

public sealed class Authorize(IPermissionCache permissionCache) : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var canRead = false;

        if (request.TeamId.HasValue)
        {
            canRead = await db
                .TeamMembers.AnyAsync(
                    a =>
                        a.MemberId == request.UserId
                        && a.TeamId == request.TeamId.Value
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.ReadIssue)),
                    ct
                )
                .ConfigureAwait(false);
        }
        if (!canRead && request.ProjectId.HasValue)
        {
            var projectPermissions = await permissionCache
                .GetProjectPermissionsAsync(request.ProjectId.Value, request.UserId, ct)
                .ConfigureAwait(false);
            canRead = projectPermissions.Contains(Permit.ReadIssue);
        }

        return canRead;
    }
}
