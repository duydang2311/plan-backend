using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ChecklistItems.Delete;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var checklistItem = await db
            .ChecklistItems.Where(a => a.Id == context.Request.Id)
            .Select(a => new
            {
                a.ParentIssueId,
                a.ParentIssue.ProjectId,
                a.ParentIssue.Project.WorkspaceId,
                a.ParentIssue.AuthorId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (checklistItem is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var isAuthor = checklistItem.AuthorId == context.Request.RequestingUserId;
        if (isAuthor)
        {
            return;
        }

        var isAssignee = await db
            .IssueAssignees.AnyAsync(
                a => a.IssueId == checklistItem.ParentIssueId && a.UserId == context.Request.RequestingUserId,
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
                    checklistItem.ProjectId,
                    context.Request.RequestingUserId,
                    Permit.DeleteChecklistItem,
                    ct
                )
                .ConfigureAwait(false)
            || await permissionCache
                .HasWorkspacePermissionAsync(
                    checklistItem.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.DeleteChecklistItem,
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
