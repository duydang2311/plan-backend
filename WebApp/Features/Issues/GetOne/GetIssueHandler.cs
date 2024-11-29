using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
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
        else if (command.ProjectId.HasValue && command.OrderNumber.HasValue)
        {
            query = query.Where(x =>
                x.ProjectId == command.ProjectId.Value && x.OrderNumber == command.OrderNumber.Value
            );
        }
        else
        {
            return new None();
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Issue, Issue>(command.Select));
        }

        var issue = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return issue is null ? new None() : issue;
    }
}
