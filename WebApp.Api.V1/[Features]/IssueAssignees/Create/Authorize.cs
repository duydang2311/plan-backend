using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAssignees.Create;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var issue = context.Request.IssueId.HasValue
            ? await db
                .Issues.Where(a => a.Id == context.Request.IssueId.Value)
                .Select(a => new { a.AuthorId, a.ProjectId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false)
            : null;
        var isAuthor = issue is not null && issue.AuthorId == context.Request.RequestingUserId;
        if (isAuthor)
        {
            return;
        }

        var canAssign =
            issue is not null
            && await permissionCache
                .HasProjectPermissionAsync(
                    issue.ProjectId,
                    context.Request.RequestingUserId,
                    Permit.CreateIssueAssignee,
                    ct
                )
                .ConfigureAwait(false);
        if (canAssign)
        {
            return;
        }

        await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
    }
}
