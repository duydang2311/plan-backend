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
    : ICommandHandler<PatchIssue, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(PatchIssue command, CancellationToken ct)
    {
        Expression<Func<SetPropertyCalls<Issue>, SetPropertyCalls<Issue>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.Description, out var description))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Description, description));
        }
        if (command.Patch.TryGetValue(a => a.Priority, out var priority))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Priority, priority));
        }
        if (command.Patch.TryGetValue(a => a.StatusId, out var statusId))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.StatusId, statusId));
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
            return ValidationFailures.Single("issueId", "Could not find issue", "not_found");
        }

        return new Success();
    }
}
