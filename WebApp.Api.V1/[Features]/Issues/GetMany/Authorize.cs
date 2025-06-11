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
        var canRead = false;

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
            Console.WriteLine("Can read " + canRead);
        }
        else if (context.Request.ProjectId.HasValue)
        {
            canRead = await permissionCache
                .HasProjectPermissionAsync(
                    context.Request.ProjectId.Value,
                    context.Request.UserId,
                    Permit.ReadIssue,
                    ct
                )
                .ConfigureAwait(false);
            Console.WriteLine("Can read " + canRead);
        }

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
