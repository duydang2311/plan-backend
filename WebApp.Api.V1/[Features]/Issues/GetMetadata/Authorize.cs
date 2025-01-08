using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetMetadata;

public sealed class Authorize : AuthorizePreProcessor<Request>
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
            canRead = await db
                .ProjectMembers.AnyAsync(
                    a =>
                        a.UserId == request.UserId
                        && a.ProjectId == request.ProjectId.Value
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.ReadIssue)),
                    ct
                )
                .ConfigureAwait(false);
        }

        return canRead;
    }
}
