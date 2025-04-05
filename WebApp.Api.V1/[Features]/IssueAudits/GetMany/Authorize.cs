using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAudits.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.IssueId.HasValue)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var issue = await db
            .Issues.Where(a => a.Id == context.Request.IssueId)
            .Select(a => new
            {
                a.AuthorId,
                a.ProjectId,
                a.Project.WorkspaceId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (issue is null)
        {
            await context.HttpContext.Response.SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }

        var canRead =
            issue.AuthorId == context.Request.RequestingUserId
            || await db
                .ProjectMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.RequestingUserId
                        && a.ProjectId == issue.ProjectId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssueAudit)),
                    ct
                )
                .ConfigureAwait(false)
            || await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.RequestingUserId
                        && a.WorkspaceId == issue.WorkspaceId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssueAudit)),
                    ct
                )
                .ConfigureAwait(false);

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
