using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAudits.Create;

public sealed class CreateIssueAuditHandler(AppDbContext db)
    : ICommandHandler<CreateIssueAudit, OneOf<ValidationFailures, IssueAudit>>
{
    public async Task<OneOf<ValidationFailures, IssueAudit>> ExecuteAsync(
        CreateIssueAudit command,
        CancellationToken ct
    )
    {
        var issueAudit = new IssueAudit
        {
            IssueId = command.IssueId,
            Action = command.Action,
            UserId = command.UserId,
            Data = command.Data,
        };

        db.Add(issueAudit);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(IssueAudit.IssueId) => ("issueId", "Invalid issueId"),
                    nameof(IssueAudit.UserId) => ("userId", "Invalid userId"),
                    _ => null,
                }
            );
        }

        return issueAudit;
    }
}
