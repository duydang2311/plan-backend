using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.SoftDeleteOne;

using Result = OneOf<ValidationFailures, Success>;

public sealed class SoftDeleteIssueCommentHandler(AppDbContext db) : ICommandHandler<SoftDeleteIssueComment, Result>
{
    public async Task<Result> ExecuteAsync(SoftDeleteIssueComment command, CancellationToken ct)
    {
        var count = await db
            .IssueComments.Where(x => x.Id == command.IssueCommentId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        return count switch
        {
            0 => ValidationFailures.Single("issueCommentId", "Issue comment does not exist", "not_found"),
            _ => new Success()
        };
    }
}
