using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.Get;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
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
