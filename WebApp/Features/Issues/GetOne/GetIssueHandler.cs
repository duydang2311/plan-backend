using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf.Types;
using OneOf;
using System.Linq.Dynamic.Core;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.GetOne;

public sealed class GetIssueHandler(AppDbContext dbContext) : ICommandHandler<GetIssue, OneOf<None, Issue>>
{
    public async Task<OneOf<None, Issue>> ExecuteAsync(GetIssue command, CancellationToken ct)
    {
        var query = dbContext.Issues.AsQueryable();
        if (command.IssueId.HasValue)
        {
            query = query.Where(x => x.Id == command.IssueId);
        }
        else if (command.TeamId.HasValue && command.OrderNumber.HasValue)
        {
            query = query.Where(x => x.TeamId == command.TeamId.Value && x.OrderNumber == command.OrderNumber.Value);
        }
        else
        {
            throw new InvalidOperationException();
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.LambdaNew<Issue>(command.Select));
        }

        var issue = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return issue is null ? new None() : issue;
    }
}
