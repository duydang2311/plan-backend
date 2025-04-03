using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.TeamMembers.UpdateRole;

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
            var isSelfUpdating = context.Request.UserId == context.Request.MemberId;
            if (isSelfUpdating)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                return;
            }

            var db = context.HttpContext.Resolve<AppDbContext>();
            var canUpdateTeamRole = await db
                .TeamMembers.AnyAsync(
                    a =>
                        a.MemberId == context.Request.UserId
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.UpdateTeamRole)),
                    cancellationToken: ct
                )
                .ConfigureAwait(false);
            if (!canUpdateTeamRole)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                return;
            }
        }
    }
}
