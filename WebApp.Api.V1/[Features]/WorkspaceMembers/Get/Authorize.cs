using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.Get;

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
            var canRead = await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.WorkspaceId == context.Request.WorkspaceId
                        && a.UserId == context.Request.UserId
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.ReadProject)),
                    ct
                )
                .ConfigureAwait(false);

            if (!canRead)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
    }
}
