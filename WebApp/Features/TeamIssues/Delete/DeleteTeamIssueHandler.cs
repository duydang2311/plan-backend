using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamIssues.Delete;

using Result = OneOf<NotFoundError, Success>;

public sealed class DeleteTeamIssueHandler(AppDbContext db) : ICommandHandler<DeleteTeamIssue, Result>
{
    public async Task<Result> ExecuteAsync(DeleteTeamIssue command, CancellationToken ct)
    {
        var count = await db
            .TeamIssues.Where(a => a.TeamId == command.TeamId && a.IssueId == command.IssueId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
