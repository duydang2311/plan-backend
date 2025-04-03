using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueComments.GetOne;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return Task.CompletedTask;
        }

        return CheckAsync(context, ct);
        static async Task CheckAsync(IPreProcessorContext<Request> context, CancellationToken ct)
        {
            Guard.Against.Null(context.Request);
            var db = context.HttpContext.Resolve<AppDbContext>();
            var canReadAsAMember = await db
                .ProjectMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.UserId
                        && a.Project.Issues.Any(b => b.Comments.Any(x => x.Id == context.Request.IssueCommentId))
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssueComment)),
                    ct
                )
                .ConfigureAwait(false);
            if (!canReadAsAMember)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
    }
}
