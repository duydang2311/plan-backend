using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Milestones.Create;

public sealed record CreateMilestoneHandler(AppDbContext db)
    : ICommandHandler<CreateMilestone, OneOf<ProjectNotFoundError, MilestoneStatusNotFoundError, Milestone>>
{
    public async Task<OneOf<ProjectNotFoundError, MilestoneStatusNotFoundError, Milestone>> ExecuteAsync(
        CreateMilestone command,
        CancellationToken ct
    )
    {
        var milestone = new Milestone
        {
            ProjectId = command.ProjectId,
            Title = command.Title,
            Description = command.Description,
            Emoji = command.Emoji,
            Color = command.Color,
            StatusId = command.StatusId,
        };

        db.Add(milestone);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            foreach (var property in e.ConstraintProperties)
            {
                if (property.Equals(nameof(Milestone.ProjectId), StringComparison.OrdinalIgnoreCase))
                {
                    return new ProjectNotFoundError();
                }
                if (property.Equals(nameof(Milestone.StatusId), StringComparison.OrdinalIgnoreCase))
                {
                    return new MilestoneStatusNotFoundError();
                }
            }
            throw;
        }
        return milestone;
    }
}
