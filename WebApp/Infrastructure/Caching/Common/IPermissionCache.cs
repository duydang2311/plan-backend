using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Caching.Common;

public interface IPermissionCache
{
    ValueTask<IReadOnlySet<string>> GetProjectPermissionsAsync(
        ProjectId projectId,
        UserId userId,
        CancellationToken ct = default
    );
    ValueTask<IReadOnlySet<string>> GetWorkspacePermissionsAsync(
        WorkspaceId workspaceId,
        UserId userId,
        CancellationToken ct = default
    );
    ValueTask<bool> HasProjectPermissionAsync(
        ProjectId projectId,
        UserId userId,
        string permission,
        CancellationToken ct = default
    );
    ValueTask<bool> HasWorkspacePermissionAsync(
        WorkspaceId workspaceId,
        UserId userId,
        string permission,
        CancellationToken ct = default
    );
    ValueTask InvalidateWorkspacePermissionAsync(
        WorkspaceId workspaceId,
        UserId userId,
        CancellationToken ct = default
    );
    ValueTask InvalidateProjectPermissionAsync(ProjectId projectId, UserId userId, CancellationToken ct = default);
}
