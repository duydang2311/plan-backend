using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Projects.Delete;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var project = await db
            .Projects.Where(a => a.Id == context.Request.ProjectId)
            .Select(a => new { a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canDelete =
            project is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(project.WorkspaceId, context.Request.UserId, Permit.DeleteProject, ct)
                .ConfigureAwait(false);
        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
