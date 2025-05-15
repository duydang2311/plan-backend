using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ChecklistItems.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.ParentIssueId.HasValue)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var issue = await db
            .Issues.Where(a => a.Id == context.Request.ParentIssueId.Value)
            .Select(a => new
            {
                a.ProjectId,
                a.Project.WorkspaceId,
                a.AuthorId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (issue is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var isAuthor = issue.AuthorId == context.Request.RequestingUserId;
        if (isAuthor)
        {
            return;
        }

        var hasPermission =
            await permissionCache
                .HasProjectPermissionAsync(
                    issue.ProjectId,
                    context.Request.RequestingUserId,
                    Permit.ReadChecklistItem,
                    ct
                )
                .ConfigureAwait(false)
            || await permissionCache
                .HasWorkspacePermissionAsync(
                    issue.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.ReadChecklistItem,
                    ct
                )
                .ConfigureAwait(false);
        if (hasPermission)
        {
            return;
        }

        var isAssignee = await db
            .IssueAssignees.AnyAsync(
                a => a.IssueId == context.Request.ParentIssueId && a.UserId == context.Request.RequestingUserId,
                ct
            )
            .ConfigureAwait(false);
        if (isAssignee)
        {
            return;
        }

        await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
    }
}
