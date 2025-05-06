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
                IsDefault = true,
                Category = StatusCategory.Pending,
            },
            new WorkspaceStatus
            {
                Value = "Todo",
                Icon = "todo",
                Category = StatusCategory.Pending,
            },
            new WorkspaceStatus
            {
                Value = "In Progress",
                Icon = "in-progress",
                Category = StatusCategory.Ongoing,
            },
            new WorkspaceStatus
            {
                Value = "Done",
                Icon = "done",
                Category = StatusCategory.Completed,
            },
            new WorkspaceStatus
            {
                Value = "Canceled",
                Icon = "canceled",
                Category = StatusCategory.Canceled,
            },
            new WorkspaceStatus
            {
                Value = "Duplicated",
                Icon = "duplicated",
                Category = StatusCategory.Canceled,
            },
        ];
}
