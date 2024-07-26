using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueComments.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        if (
            !await dbContext
                .TeamMembers.AnyAsync(
                    x =>
                        x.MemberId == context.Request.UserId
                        && x.Team.Issues.Any(x => x.Id == context.Request.IssueId)
                        && x.Role.Permissions.Any(x => x.Permission.Equals(Permit.ReadIssue)),
                    ct
                )
                .ConfigureAwait(false)
        )
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
