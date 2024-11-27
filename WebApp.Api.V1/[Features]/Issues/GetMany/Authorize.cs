using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetMany;

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
            var canRead = true;

            if (context.Request.TeamId is not null)
            {
                canRead = await db
                    .TeamMembers.AnyAsync(
                        a =>
                            a.MemberId == context.Request.UserId
                            && a.TeamId == context.Request.TeamId
                            && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.ReadIssue)),
                        ct
                    )
                    .ConfigureAwait(false);
            }

            if (!canRead)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
        ;
    }
}
