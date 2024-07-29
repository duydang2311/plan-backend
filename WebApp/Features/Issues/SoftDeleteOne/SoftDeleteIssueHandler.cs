using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.SoftDeleteOne;

using Result = OneOf<ValidationFailures, Success>;

public sealed record SoftDeleteIssueHandler(AppDbContext dbContext) : ICommandHandler<SoftDeleteIssue, Result>
{
    public async Task<Result> ExecuteAsync(SoftDeleteIssue command, CancellationToken ct)
    {
        var count = await dbContext
            .Issues.Where(x => x.Id == command.IssueId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        return count switch
        {
            0 => ValidationFailures.Single("issueId", "Issue does not exist", "not_found"),
            _ => new Success(),
        };
    }
}
