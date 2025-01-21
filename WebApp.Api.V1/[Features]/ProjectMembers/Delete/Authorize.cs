using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMembers.Delete;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }
        var db = context.HttpContext.Resolve<AppDbContext>();
        var data = await db
            .ProjectMembers.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.ProjectId, a.Project.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (data is null)
        {
            await context.HttpContext.Response.SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }
        var canDelete =
            await db
                .ProjectMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.RequestingUserId
                        && a.ProjectId == data.ProjectId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.DeleteProjectMember)),
                    ct
                )
                .ConfigureAwait(false)
            || await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.UserId == context.Request.RequestingUserId
                        && a.WorkspaceId == data.WorkspaceId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.DeleteProjectMember)),
                    ct
                )
                .ConfigureAwait(false);
        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
