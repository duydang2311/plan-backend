using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Projects.Delete;

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
            var canDelete = await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.UserId
                        && a.Workspace.Projects.Any(a => a.Id == context.Request.ProjectId)
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.DeleteProject)),
                    ct
                )
                .ConfigureAwait(false);
            if (!canDelete)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
    }
}
