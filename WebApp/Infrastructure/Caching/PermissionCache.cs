using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Infrastructure.Caching;

public sealed class PermissionCache(HybridCache cache, IDbContextFactory<AppDbContext> dbFactory) : IPermissionCache
{
    public async ValueTask<IReadOnlySet<string>> GetProjectPermissionsAsync(
        ProjectId projectId,
        UserId userId,
        CancellationToken ct = default
    )
    {
        return await cache
            .GetOrCreateAsync(
                CacheKeys.ProjectPermissions(projectId, userId),
                new GetProjectPermissionsState(dbFactory, projectId, userId),
                static (state, ct) =>
                    GetProjectPermissionsInternalAsync(state.DbFactory, state.ProjectId, state.UserId, ct),
                cancellationToken: ct
            )
            .ConfigureAwait(false);
    }

    public async ValueTask<IReadOnlySet<string>> GetWorkspacePermissionsAsync(
        WorkspaceId workspaceId,
        UserId userId,
        CancellationToken ct = default
    )
    {
        return await cache
            .GetOrCreateAsync(
                CacheKeys.WorkspacePermissions(workspaceId, userId),
                new GetWorkspacePermissionsState(dbFactory, workspaceId, userId),
                static (state, ct) =>
                    GetWorkspacePermissionsInternalAsync(state.DbFactory, state.WorkspaceId, state.UserId, ct),
                cancellationToken: ct
            )
            .ConfigureAwait(false);
    }

    static async ValueTask<HashSet<string>> GetProjectPermissionsInternalAsync(
        IDbContextFactory<AppDbContext> dbFactory,
        ProjectId projectId,
        UserId userId,
        CancellationToken cancellationToken
    )
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        return await db
            .ProjectMembers.Where(a => a.ProjectId == projectId && a.UserId == userId)
            .SelectMany(a => a.Role.Permissions.Select(b => b.Permission))
            .ToHashSetAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    static async ValueTask<HashSet<string>> GetWorkspacePermissionsInternalAsync(
        IDbContextFactory<AppDbContext> dbFactory,
        WorkspaceId workspaceId,
        UserId userId,
        CancellationToken cancellationToken
    )
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        return await db
            .WorkspaceMembers.Where(a => a.WorkspaceId == workspaceId && a.UserId == userId)
            .SelectMany(a => a.Role.Permissions.Select(b => b.Permission))
            .ToHashSetAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<bool> HasProjectPermissionAsync(
        ProjectId projectId,
        UserId userId,
        string permission,
        CancellationToken ct = default
    )
    {
        var permissions = await GetProjectPermissionsAsync(projectId, userId, ct).ConfigureAwait(false);
        return permissions.Contains(permission);
    }

    public async ValueTask<bool> HasWorkspacePermissionAsync(
        WorkspaceId workspaceId,
        UserId userId,
        string permission,
        CancellationToken ct = default
    )
    {
        var permissions = await GetWorkspacePermissionsAsync(workspaceId, userId, ct).ConfigureAwait(false);
        return permissions.Contains(permission);
    }

    public async ValueTask InvalidateWorkspacePermissionAsync(
        WorkspaceId workspaceId,
        UserId userId,
        CancellationToken ct = default
    )
    {
        await cache.RemoveAsync(CacheKeys.WorkspacePermissions(workspaceId, userId), ct).ConfigureAwait(false);
    }

    public async ValueTask InvalidateProjectPermissionAsync(
        ProjectId projectId,
        UserId userId,
        CancellationToken ct = default
    )
    {
        await cache.RemoveAsync(CacheKeys.ProjectPermissions(projectId, userId), ct).ConfigureAwait(false);
    }

    sealed class GetProjectPermissionsState(
        IDbContextFactory<AppDbContext> dbFactory,
        ProjectId projectId,
        UserId userId
    )
    {
        public IDbContextFactory<AppDbContext> DbFactory { get; } = dbFactory;
        public ProjectId ProjectId { get; } = projectId;
        public UserId UserId { get; } = userId;
    }

    sealed class GetWorkspacePermissionsState(
        IDbContextFactory<AppDbContext> dbFactory,
        WorkspaceId workspaceId,
        UserId userId
    )
    {
        public IDbContextFactory<AppDbContext> DbFactory { get; } = dbFactory;
        public WorkspaceId WorkspaceId { get; } = workspaceId;
        public UserId UserId { get; } = userId;
    }
}
