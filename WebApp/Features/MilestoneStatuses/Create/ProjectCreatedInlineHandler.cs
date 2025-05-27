using WebApp.Domain.Constants;
using WebApp.Domain.Events;

namespace WebApp.Features.MilestoneStatuses.Create;

public static class ProjectCreatedInlineHandler
{
    public static void Handle(ProjectCreatedInline projectCreated)
    {
        projectCreated.Db.AddRange(
            MilestoneStatusDefaults.MilestoneStatuses.Select(a => a with { ProjectId = projectCreated.ProjectId })
        );
    }
}
