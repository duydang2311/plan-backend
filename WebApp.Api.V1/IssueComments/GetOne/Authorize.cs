using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueComments.GetOne;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var canReadAsAMember = await db
            .TeamMembers.AnyAsync(
                x =>
                    x.MemberId == context.Request.UserId
                    && x.Team.Issues.Any(x => x.Comments.Any(x => x.Id == context.Request.IssueCommentId))
                    && x.Role.Permissions.Any(x => x.Permission.Equals(Permit.ReadIssueComment)),
                ct
            )
            .ConfigureAwait(false);
        Console.WriteLine($"Authorize {context.Request} - Result: {canReadAsAMember}");
        if (!canReadAsAMember)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
