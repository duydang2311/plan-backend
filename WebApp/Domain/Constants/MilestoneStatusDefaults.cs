using FractionalIndexing;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public static class MilestoneStatusDefaults
{
    public static IEnumerable<MilestoneStatus> MilestoneStatuses
    {
        get
        {
            var ranks = OrderKeyGenerator.GenerateNKeysBetween(
                null,
                null,
                5,
                FractionalIndexingDefaults.BASE_95_DIGITS
            );
            return
            [
                new MilestoneStatus
                {
                    Value = "Not started",
                    Icon = "not-started",
                    Rank = ranks[0],
                    IsDefault = true,
                    Category = MilestoneStatusCategory.Pending,
                },
                new MilestoneStatus
                {
                    Value = "In Progress",
                    Icon = "in-progress",
                    Rank = ranks[1],
                    Category = MilestoneStatusCategory.Ongoing,
                },
                new MilestoneStatus
                {
                    Value = "Completed",
                    Icon = "completed",
                    Rank = ranks[2],
                    Category = MilestoneStatusCategory.Completed,
                },
                new MilestoneStatus
                {
                    Value = "Canceled",
                    Icon = "canceled",
                    Rank = ranks[3],
                    Category = MilestoneStatusCategory.Canceled,
                },
                new MilestoneStatus
                {
                    Value = "Paused",
                    Icon = "paused",
                    Rank = ranks[4],
                    Category = MilestoneStatusCategory.Paused,
                },
            ];
        }
    }
}
