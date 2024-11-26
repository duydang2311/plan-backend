using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceStatuses.Patch;

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
            var canUpdate = await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.UserId
                        && a.Workspace.Statuses.Any(a => a.Id == context.Request.StatusId)
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.UpdateWorkspaceStatus)),
                    ct
                )
                .ConfigureAwait(false);
            if (!canUpdate)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
    }
}
