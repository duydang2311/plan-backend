using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.Delete;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return Task.CompletedTask;
        }

        return AuthorizeAsync(context, ct);
        static async Task AuthorizeAsync(IPreProcessorContext<Request> context, CancellationToken ct)
        {
            Guard.Against.Null(context.Request);
            var db = context.HttpContext.Resolve<AppDbContext>();
            var canDelete = await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.UserId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.DeleteWorkspaceMember)),
                    ct
                )
                .ConfigureAwait(false);

            if (!canDelete)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            }
        }
        ;
    }
}
