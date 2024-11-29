using System.Data;
using FastEndpoints;
using FractionalIndexing;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.Create;

using Result = OneOf<ValidationFailures, Issue>;

public sealed class CreateIssueHandler(AppDbContext db, IServiceProvider serviceProvider)
    : ICommandHandler<CreateIssue, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssue command, CancellationToken ct)
    {
        await using var transaction = await db
            .Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct)
            .ConfigureAwait(false);

        var lastIssueWithSameStatus = await db
            .Issues.Where(a => a.ProjectId == command.ProjectId && a.StatusId == command.StatusId)
            .Select(a => new { a.StatusRank })
            .OrderByDescending(a => a.StatusRank)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var issue = new Issue
        {
            Id = IdHelper.NewIssueId(),
            AuthorId = command.AuthorId,
            ProjectId = command.ProjectId,
            Title = command.Title,
            Description = command.Description,
            StatusId = command.StatusId,
            StatusRank = OrderKeyGenerator.GenerateKeyBetween(lastIssueWithSameStatus?.StatusRank, null),
        };

        db.Add(issue);
        await new IssueCreated
        {
            ServiceProvider = serviceProvider,
            AuthorId = command.AuthorId,
            ProjectId = command.ProjectId,
            Issue = issue
        }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(false);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        await transaction.CommitAsync(ct).ConfigureAwait(false);

        return issue;
    }
}
