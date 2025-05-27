using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public static class MilestoneStatusDefaults
{
    public static IEnumerable<MilestoneStatus> MilestoneStatuses =>
        [
            new MilestoneStatus
            {
                Value = "Not started",
                Icon = "not-started",
                IsDefault = true,
                Category = MilestoneStatusCategory.Pending,
            },
            new MilestoneStatus
            {
                Value = "In Progress",
                Icon = "in-progress",
                Category = MilestoneStatusCategory.Ongoing,
            },
            new MilestoneStatus
            {
                Value = "Done",
                Icon = "done",
                Category = MilestoneStatusCategory.Completed,
            },
            new MilestoneStatus
            {
                Value = "Canceled",
                Icon = "canceled",
                Category = MilestoneStatusCategory.Canceled,
            },
            new MilestoneStatus
            {
                Value = "Paused",
                Icon = "paused",
                Category = MilestoneStatusCategory.Paused,
            },
        ];
}
