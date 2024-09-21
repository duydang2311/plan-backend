using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Patch;

public sealed class PatchIssueCommentHandler(AppDbContext dbContext)
    : ICommandHandler<PatchIssueComment, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(PatchIssueComment command, CancellationToken ct)
    {
        Expression<Func<SetPropertyCalls<IssueComment>, SetPropertyCalls<IssueComment>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.Content, out var content))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Content, content));
        }

        if (updateEx is null)
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        var count = await dbContext
            .IssueComments.Where(a => a.Id == command.IssueCommentId)
            .ExecuteUpdateAsync(updateEx, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return ValidationFailures.Single("issueCommentId", "Could not find comment", "no_reference");
        }

        return new Success();
    }
}
