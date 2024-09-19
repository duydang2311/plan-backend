using System.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.PatchStatus;

public sealed class PatchIssueStatusHandler(AppDbContext dbContext)
    : ICommandHandler<PatchIssueStatus, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(PatchIssueStatus command, CancellationToken ct)
    {
        if (!command.OrderByStatus.HasValue)
        {
            var count = await dbContext
                .Issues.Where(a => a.Id == command.IssueId)
                .ExecuteUpdateAsync(a => a.SetProperty(b => b.StatusId, command.StatusId), ct)
                .ConfigureAwait(false);
            return count == 0
                ? ValidationFailures.Single("issueId", "Could not find issue", "not_found")
                : new Success();
        }

        await using var transaction = await dbContext
            .Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, ct)
            .ConfigureAwait(false);
        var issue = await dbContext
            .Issues.Where(x => x.Id == command.IssueId)
            .Select(a => new { a.StatusId, a.OrderByStatus })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (issue is null)
        {
            await transaction.CommitAsync(ct).ConfigureAwait(false);
            return ValidationFailures.Single("issueId", "Could not find issue", "not_found");
        }

        if ((issue.StatusId, issue.OrderByStatus) == (command.StatusId, command.OrderByStatus))
        {
            await transaction.CommitAsync(ct).ConfigureAwait(false);
            return new Success();
        }

        StatusId? newStatusId = command.StatusId.Value == -1 ? null : command.StatusId;
        await dbContext
            .Issues.Where(a =>
                a.Id == command.IssueId || (a.StatusId == newStatusId && a.OrderByStatus <= command.OrderByStatus)
            )
            .ExecuteUpdateAsync(
                a =>
                    a.SetProperty(b => b.StatusId, b => b.Id == command.IssueId ? newStatusId : b.StatusId)
                        .SetProperty(
                            b => b.OrderByStatus,
                            b => b.Id == command.IssueId ? command.OrderByStatus : b.OrderByStatus - 1
                        ),
                ct
            )
            .ConfigureAwait(false);

        await transaction.CommitAsync(ct).ConfigureAwait(false);

        return new Success();
    }
}
