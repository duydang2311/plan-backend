using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAudits.GetMany;

public sealed class GetIssueAuditsHandler(AppDbContext db) : ICommandHandler<GetIssueAudits, PaginatedList<IssueAudit>>
{
    public async Task<PaginatedList<IssueAudit>> ExecuteAsync(GetIssueAudits command, CancellationToken ct)
    {
        var query = db.IssueAudits.Where(a => a.IssueId == command.IssueId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<IssueAudit, IssueAudit>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
