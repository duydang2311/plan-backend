using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ResourceFiles.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.ResourceId.HasValue)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var resource = await db
            .Resources.Where(a => a.Id == context.Request.ResourceId.Value)
            .Select(a => new
            {
                WorkspaceId = a.WorkspaceResource == null ? null : (WorkspaceId?)a.WorkspaceResource.WorkspaceId,
                ProjectId = a.ProjectResource == null ? null : (ProjectId?)a.ProjectResource.ProjectId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var canRead =
            resource is not null
            && (
                (
                    resource.WorkspaceId.HasValue
                    && await permissionCache
                        .HasWorkspacePermissionAsync(
                            resource.WorkspaceId.Value,
                            context.Request.RequestingUserId,
                            Permit.ReadWorkspaceResourceFile,
                            ct
                        )
                        .ConfigureAwait(false)
                )
                || (
                    resource.ProjectId.HasValue
                    && await permissionCache
                        .HasProjectPermissionAsync(
                            resource.ProjectId.Value,
                            context.Request.RequestingUserId,
                            Permit.ReadProjectResourceFile,
                            ct
                        )
                        .ConfigureAwait(false)
                )
            );

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
