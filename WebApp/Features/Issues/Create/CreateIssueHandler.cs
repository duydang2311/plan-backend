using System.Data;
using EntityFramework.Exceptions.Common;
using FastEndpoints;
using FractionalIndexing;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.Issues.Create;

using Result = OneOf<ValidationFailures, Issue>;

public sealed class CreateIssueHandler(AppDbContext db, IDbContextOutbox outbox) : ICommandHandler<CreateIssue, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssue command, CancellationToken ct)
    {
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
        try
        {
            outbox.Enroll(db);
            await outbox
                .PublishAsync(new IssueCreated { IssueId = issue.Id, ProjectId = issue.ProjectId })
                .ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(Issue.AuthorId) => ("authorId", "Invalid author reference"),
                    nameof(Issue.ProjectId) => ("projectId", "Invalid project reference"),
                    _ => null,
                }
            );
        }

        return issue;
    }
}
