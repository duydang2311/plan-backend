using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ResourceFiles.CreateBatch;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var resourceIds = context
            .Request.Files?.Where(a => a.ResourceId.HasValue)
            .Select(a => a.ResourceId!.Value)
            .Distinct()
            .ToList();
        if (resourceIds is null || resourceIds.Count == 0)
        {
            return;
        }

        var resources = await db
            .Resources.Where(a => resourceIds.Contains(a.Id))
            .Select(a => new
            {
                WorkspaceId = a.WorkspaceResource == null ? null : (WorkspaceId?)a.WorkspaceResource.WorkspaceId,
                ProjectId = a.ProjectResource == null ? null : (ProjectId?)a.ProjectResource.ProjectId,
            })
            .ToListAsync(ct)
            .ConfigureAwait(false);

        var workspaceIds = resources
            .Where(a => a.WorkspaceId.HasValue)
            .Select(a => a.WorkspaceId!.Value)
            .Distinct()
            .ToList();

        var projectIds = resources.Where(a => a.ProjectId.HasValue).Select(a => a.ProjectId!.Value).Distinct().ToList();

        foreach (var id in workspaceIds)
        {
            if (
                !await permissionCache
                    .HasWorkspacePermissionAsync(
                        id,
                        context.Request.RequestingUserId,
                        Permit.CreateWorkspaceResourceFile,
                        ct
                    )
                    .ConfigureAwait(false)
            )
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                return;
            }
        }

        foreach (var id in projectIds)
        {
            if (
                !await permissionCache
                    .HasProjectPermissionAsync(
                        id,
                        context.Request.RequestingUserId,
                        Permit.CreateProjectResourceFile,
                        ct
                    )
                    .ConfigureAwait(false)
            )
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                return;
            }
        }
    }
}
