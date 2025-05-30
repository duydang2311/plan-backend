using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Milestones.Patch;

public sealed record PatchMilestoneHandler(AppDbContext db)
    : ICommandHandler<PatchMilestone, OneOf<NotFoundError, InvalidPatchError, Success>>
{
    public async Task<OneOf<NotFoundError, InvalidPatchError, Success>> ExecuteAsync(
        PatchMilestone command,
        CancellationToken ct
    )
    {
        Expression<Func<SetPropertyCalls<Milestone>, SetPropertyCalls<Milestone>>>? expression = default;

        if (command.Patch.TryGetValue(a => a.Title, out var title) && !string.IsNullOrEmpty(title))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.Title, title));
        }
        if (command.Patch.TryGetValue(a => a.Emoji, out var emoji) && !string.IsNullOrEmpty(emoji))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.Emoji, emoji));
        }
        if (command.Patch.TryGetValue(a => a.Color, out var color) && !string.IsNullOrEmpty(color))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.Color, color));
        }
        if (command.Patch.TryGetValue(a => a.Description, out var description))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.Description, description));
        }
        if (command.Patch.TryGetValue(a => a.StatusId, out var statusId) && statusId.HasValue)
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.StatusId, statusId.Value));
        }
        if (command.Patch.TryGetValue(a => a.EndTime, out var endTime) && endTime.HasValue)
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.EndTime, endTime.Value));
        }
        if (command.Patch.TryGetValue(a => a.EndTimeZone, out var endTimeZone))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(a => a.EndTimeZone, endTimeZone));
        }

        if (expression is null)
        {
            return new InvalidPatchError();
        }

        var count = await db
            .Milestones.Where(a => a.Id == command.Id)
            .ExecuteUpdateAsync(expression, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
