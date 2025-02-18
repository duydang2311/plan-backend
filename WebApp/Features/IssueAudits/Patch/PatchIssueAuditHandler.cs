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

namespace WebApp.Features.IssueAudits.Patch;

public sealed class PatchIssueAuditHandler(AppDbContext db)
    : ICommandHandler<PatchIssueAudit, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(PatchIssueAudit command, CancellationToken ct)
    {
        Expression<Func<SetPropertyCalls<IssueAudit>, SetPropertyCalls<IssueAudit>>>? expression = default;

        if (command.Patch.TryGetValue(a => a.Data, out var data))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(b => b.Data, data));
        }

        if (expression is null)
        {
            return new Success();
        }
        var count = await db
            .IssueAudits.Where(a => a.Id == command.Id)
            .ExecuteUpdateAsync(expression, ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
