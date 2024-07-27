using FastEndpoints;
using Json.Patch;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.Patch;

public sealed class PatchIssueHandler(AppDbContext dbContext)
    : ICommandHandler<PatchIssue, OneOf<ValidationFailures, Issue>>
{
    public async Task<OneOf<ValidationFailures, Issue>> ExecuteAsync(PatchIssue command, CancellationToken ct)
    {
        var issue = await dbContext.Issues.FirstOrDefaultAsync(x => x.Id == command.IssueId, ct).ConfigureAwait(false);
        if (issue is null)
        {
            return ValidationFailures.Single("issueId", "Could not find issue", "not_found");
        }

        try
        {
            issue = new JsonPatch(
                command.Patch.Operations.Where(static x =>
                    x.Path[0]
                        .EqualsEither(
                            [nameof(Issue.Description), nameof(Issue.Title)],
                            StringComparison.OrdinalIgnoreCase
                        )
                )
            ).Apply(issue)!;
        }
        catch
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        await dbContext
            .Issues.Where(x => x.Id == command.IssueId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(x => x.Description, issue.Description), ct)
            .ConfigureAwait(false);

        return issue;
    }
}
