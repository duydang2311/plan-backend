using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public static class StatusDefaults
{
    public static IEnumerable<WorkspaceStatus> WorkspaceStatuses =>
        [
            new WorkspaceStatus { Value = "Backlog", IsDefault = true },
            new WorkspaceStatus { Value = "Todo" },
            new WorkspaceStatus { Value = "In Progress" },
            new WorkspaceStatus { Value = "Done" },
            new WorkspaceStatus { Value = "Canceled" },
            new WorkspaceStatus { Value = "Duplicated" },
        ];
}
