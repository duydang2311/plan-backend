using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.DeleteOne;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return Task.CompletedTask;
        }

        return CheckAsync(context, ct);
        static async Task CheckAsync(IPreProcessorContext<Request> context, CancellationToken ct)
        {
            Guard.Against.Null(context.Request);
            var db = context.HttpContext.Resolve<AppDbContext>();
            var isAuthor = await db
                .Issues.AnyAsync(x => x.Id == context.Request.IssueId && x.AuthorId == context.Request.UserId, ct)
                .ConfigureAwait(false);
            if (!isAuthor)
            {
                var canDelete = await db
                    .TeamMembers.AnyAsync(
                        x =>
                            x.MemberId == context.Request.UserId
                            && x.Team.Issues.Any(x => x.Id == context.Request.IssueId)
                            && x.Role.Permissions.Any(x => x.Permission == Permit.DeleteIssue),
                        ct
                    )
                    .ConfigureAwait(false);
                if (!canDelete)
                {
                    await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                }
            }
        };
    }
}
