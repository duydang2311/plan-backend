using FastEndpoints;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.Create;

using Result = OneOf<ValidationFailures, Issue>;

public sealed class CreateIssueHandler(AppDbContext dbContext, IServiceProvider serviceProvider)
    : ICommandHandler<CreateIssue, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssue command, CancellationToken ct)
    {
        var issue = new Issue
        {
            Id = IdHelper.NewIssueId(),
            AuthorId = command.AuthorId,
            TeamId = command.TeamId,
            Title = command.Title,
            Description = command.Description,
        };
        dbContext.Add(issue);
        await new IssueCreated
        {
            ServiceProvider = serviceProvider,
            AuthorId = command.AuthorId,
            TeamId = command.TeamId,
            Issue = issue
        }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(false);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return issue;
    }
}
