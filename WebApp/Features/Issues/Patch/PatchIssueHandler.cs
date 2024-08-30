using System.Linq.Expressions;
using FastEndpoints;
using Json.Patch;
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
        var any = await dbContext.Issues.AnyAsync(x => x.Id == command.IssueId, ct).ConfigureAwait(false);
        if (!any)
        {
            return ValidationFailures.Single("issueId", "Could not find issue", "not_found");
        }

        Expression<Func<SetPropertyCalls<Issue>, SetPropertyCalls<Issue>>>? updateEx = default;
        foreach (
            var op in command.Patch.Operations.Where(a =>
                a.Op == OperationType.Replace && a.Path.Count > 0 && a.Value is not null
            )
        )
        {
            if (op.Path[0].Equals(nameof(Issue.Description), StringComparison.OrdinalIgnoreCase))
            {
                updateEx = ExpressionHelper.Append(
                    updateEx,
                    a => a.SetProperty(a => a.Description, op.Value!.GetValue<string>())
                );
            }
            else if (op.Path[0].Equals(nameof(Issue.Title), StringComparison.OrdinalIgnoreCase))
            {
                updateEx = ExpressionHelper.Append(
                    updateEx,
                    a => a.SetProperty(a => a.Title, op.Value!.GetValue<string>())
                );
            }
        }

        if (updateEx is not null)
        {
            await dbContext
                .Issues.Where(a => a.Id == command.IssueId)
                .ExecuteUpdateAsync(updateEx, ct)
                .ConfigureAwait(false);
        }

        return new Success();
    }
}
