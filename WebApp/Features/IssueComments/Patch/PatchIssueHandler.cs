using FastEndpoints;
using Json.Patch;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Patch;

public sealed class PatchIssueCommentHandler(AppDbContext dbContext)
    : ICommandHandler<PatchIssueComment, OneOf<ValidationFailures, IssueComment>>
{
    public async Task<OneOf<ValidationFailures, IssueComment>> ExecuteAsync(
        PatchIssueComment command,
        CancellationToken ct
    )
    {
        var issue = await dbContext
            .IssueComments.FirstOrDefaultAsync(x => x.Id == command.IssueCommentId, ct)
            .ConfigureAwait(false);
        if (issue is null)
        {
            return ValidationFailures.Single("issueCommentId", "Could not find issue comment", "not_found");
        }

        try
        {
            issue = new JsonPatch(
                command.Patch.Operations.Where(static x =>
                    x.Path[0].EqualsEither([nameof(IssueComment.Content)], StringComparison.OrdinalIgnoreCase)
                )
            ).Apply(issue)!;
        }
        catch
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        await dbContext
            .IssueComments.Where(x => x.Id == command.IssueCommentId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(x => x.Content, issue.Content), ct)
            .ConfigureAwait(false);

        return issue;
    }
}
