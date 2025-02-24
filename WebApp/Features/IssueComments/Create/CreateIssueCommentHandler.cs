using System.Text.Json;
using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Create;

using Result = OneOf<ValidationFailures, Success>;

public sealed class CreateIssueCommentHandler(AppDbContext dbContext) : ICommandHandler<CreateIssueComment, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssueComment command, CancellationToken ct)
    {
        var audit = new IssueAudit
        {
            Action = IssueAuditAction.Comment,
            IssueId = command.IssueId,
            UserId = command.AuthorId,
            Data = JsonSerializer.SerializeToDocument(new { content = command.Content }).Deserialize<JsonDocument>(),
        };

        dbContext.Add(audit);
        try
        {
            await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property => (property, $"Invalid {property} reference"));
        }

        await new IssueCommentCreated { IssueAudit = audit }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(false);

        return new Success();
    }
}
