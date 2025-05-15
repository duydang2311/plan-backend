using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ChecklistItems.Create;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var parentIssue = await db
            .Issues.Where(a => a.Id == context.Request.ParentIssueId)
            .Select(a => new
            {
                a.AuthorId,
                a.ProjectId,
                a.Project.WorkspaceId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (parentIssue is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var isAuthor = parentIssue.AuthorId == context.Request.RequestingUserId;
        if (isAuthor)
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

        var hasPermission =
            await permissionCache
                .HasProjectPermissionAsync(
                    parentIssue.ProjectId,
                    context.Request.RequestingUserId,
                    Permit.CreateChecklistItem,
                    ct
                )
                .ConfigureAwait(false)
            || await permissionCache
                .HasWorkspacePermissionAsync(
                    parentIssue.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.CreateChecklistItem,
                    ct
                )
                .ConfigureAwait(false);
        if (hasPermission)
        {
            return;
        }

        await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
    }
}
