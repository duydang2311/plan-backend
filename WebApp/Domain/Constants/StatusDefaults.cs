using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public static class StatusDefaults
{
    public static IEnumerable<WorkspaceStatus> WorkspaceStatuses =>
        [
            new WorkspaceStatus
            {
                Value = "Backlog",
                Icon = "backlog",
                IsDefault = true
            },
            new WorkspaceStatus { Value = "Todo", Icon = "todo", },
            new WorkspaceStatus { Value = "In Progress", Icon = "in-progress" },
            new WorkspaceStatus { Value = "Done", Icon = "done" },
            new WorkspaceStatus { Value = "Canceled", Icon = "canceled" },
            new WorkspaceStatus { Value = "Duplicated", Icon = "duplicated" },
        ];
}
