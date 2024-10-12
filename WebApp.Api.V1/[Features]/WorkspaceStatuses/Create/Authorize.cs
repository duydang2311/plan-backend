using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceStatuses.Create;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canCreate = await db
            .WorkspaceMembers.AnyAsync(
                a =>
                    a.WorkspaceId == context.Request.WorkspaceId
                    && a.UserId == context.Request.UserId
                    && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.CreateWorkspaceStatus)),
                ct
            )
            .ConfigureAwait(false);

        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
