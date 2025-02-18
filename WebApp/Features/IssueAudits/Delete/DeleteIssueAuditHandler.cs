using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAudits.Delete;

public sealed class DeleteIssueAuditHandler(AppDbContext db)
    : ICommandHandler<DeleteIssueAudit, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteIssueAudit command, CancellationToken ct)
    {
        var count = await db.IssueAudits.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
