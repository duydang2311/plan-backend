using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ResourceFiles.Delete;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var resourceFile = await db
            .ResourceFiles.Where(a => a.Id == context.Request.Id)
            .Select(a => new
            {
                WorkspaceId = a.Resource.WorkspaceResource == null
                    ? null
                    : (WorkspaceId?)a.Resource.WorkspaceResource.WorkspaceId,
                ProjectId = a.Resource.ProjectResource == null
                    ? null
                    : (ProjectId?)a.Resource.ProjectResource.ProjectId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canDelete =
            resourceFile is not null
            && (
                (
                    resourceFile.WorkspaceId.HasValue
                    && await permissionCache
                        .HasWorkspacePermissionAsync(
                            resourceFile.WorkspaceId.Value,
                            context.Request.RequestingUserId,
                            Permit.DeleteWorkspaceResourceFile,
                            ct
                        )
                        .ConfigureAwait(false)
                )
                || resourceFile.ProjectId.HasValue
                    && await permissionCache
                        .HasProjectPermissionAsync(
                            resourceFile.ProjectId.Value,
                            context.Request.RequestingUserId,
                            Permit.DeleteProjectResourceFile,
                            ct
                        )
                        .ConfigureAwait(false)
            );

        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
