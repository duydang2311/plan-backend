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

namespace WebApp.Features.Issues.Patch;

public sealed class PatchIssueHandler(AppDbContext dbContext)
    : ICommandHandler<PatchIssue, OneOf<ValidationFailures, NotFoundError, Success>>
{
    public async Task<OneOf<ValidationFailures, NotFoundError, Success>> ExecuteAsync(
        PatchIssue command,
        CancellationToken ct
    )
    {
        Expression<Func<SetPropertyCalls<Issue>, SetPropertyCalls<Issue>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.Title, out var title) && !string.IsNullOrEmpty(title))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Title, title));
        }
        if (command.Patch.TryGetValue(a => a.Description, out var description))
        {
            var previewDescription = !string.IsNullOrEmpty(description)
                ? HtmlHelper.ConvertToPlainText(description, 256)
                : null;
            if (previewDescription != null && previewDescription.Length >= 256)
            {
                previewDescription = previewDescription[..256];
            }
            updateEx = ExpressionHelper.Append(
                updateEx,
                a =>
                    a.SetProperty(a => a.Description, description)
                        .SetProperty(a => a.PreviewDescription, previewDescription)
            );
        }
        if (command.Patch.TryGetValue(a => a.Priority, out var priority))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Priority, priority));
        }
        if (command.Patch.TryGetValue(a => a.StatusId, out var statusId))
        {
            updateEx = ExpressionHelper.Append(
                updateEx,
                a => a.SetProperty(a => a.StatusId, statusId.Value == -1 ? null : statusId)
            );
        }
        if (command.Patch.TryGetValue(a => a.StatusRank, out var statusRank) && statusRank is not null)
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.StatusRank, statusRank));
        }

        if (updateEx is null)
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        var count = await dbContext
            .Issues.Where(a => a.Id == command.IssueId)
            .ExecuteUpdateAsync(updateEx, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
