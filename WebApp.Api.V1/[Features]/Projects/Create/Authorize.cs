using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Projects.Create;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var workspace = await db
            .Workspaces.Where(a => a.Id == context.Request.WorkspaceId)
            .Select(a => new { a.Id })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canCreate =
            workspace is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(workspace.Id, context.Request.UserId, Permit.CreateProject, ct)
                .ConfigureAwait(false);
        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
