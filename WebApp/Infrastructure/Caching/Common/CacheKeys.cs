using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Caching.Common;

public static class CacheKeys
{
    public static string ProjectPermissions(ProjectId projectId, UserId userId) => $"pj-perms:{projectId}:{userId}";

    public static string WorkspacePermissions(WorkspaceId workspaceId, UserId userId) =>
        $"ws-perms:{workspaceId}:{userId}";
}
