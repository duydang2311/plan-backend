using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAssignees.Delete;

using Result = OneOf<NotFoundError, Success>;

public sealed record DeleteIssueAssigneeHandler(AppDbContext db) : ICommandHandler<DeleteIssueAssignee, Result>
{
    public async Task<Result> ExecuteAsync(DeleteIssueAssignee command, CancellationToken ct)
    {
        var count = await db
            .IssueAssignees.Where(a => a.IssueId == command.IssueId && a.UserId == command.UserId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
