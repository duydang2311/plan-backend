using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Milestones.Create;

public sealed record CreateMilestoneHandler(AppDbContext db)
    : ICommandHandler<CreateMilestone, OneOf<NotFoundError, Milestone>>
{
    public async Task<OneOf<NotFoundError, Milestone>> ExecuteAsync(CreateMilestone command, CancellationToken ct)
    {
        var previewDescription = string.IsNullOrEmpty(command.Description)
            ? null
            : HtmlHelper.ConvertToPlainText(command.Description, 256);
        var milestone = new Milestone
        {
            ProjectId = command.ProjectId,
            Title = command.Title,
            Description = command.Description,
            PreviewDescription = previewDescription,
            Emoji = command.Emoji,
            Color = command.Color,
        };

        db.Add(milestone);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            if (
                e.ConstraintProperties.Any(b =>
                    b.Equals(nameof(Milestone.ProjectId), StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return new NotFoundError();
            }
            throw;
        }
        return milestone;
    }
}
