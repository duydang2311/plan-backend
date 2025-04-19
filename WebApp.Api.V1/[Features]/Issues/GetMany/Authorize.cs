using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canRead = true;

        if (context.Request.TeamId.HasValue)
        {
            canRead = await db
                .TeamMembers.AnyAsync(
                    a =>
                        a.MemberId == context.Request.UserId
                        && a.TeamId == context.Request.TeamId.Value
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.ReadIssue)),
                    ct
                )
                .ConfigureAwait(false);
        }
        else if (context.Request.ProjectId.HasValue)
        {
            var projectPermissions = await permissionCache
                .GetProjectPermissionsAsync(context.Request.ProjectId.Value, context.Request.UserId, ct)
                .ConfigureAwait(false);
            canRead = projectPermissions.Contains(Permit.ReadIssue);
        }

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
