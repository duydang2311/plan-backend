using System.Text.Json;
using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.IssueComments.Create;

using Result = OneOf<ValidationFailures, ServerError, Success>;

public sealed class CreateIssueCommentHandler(
    AppDbContext db,
    IDbContextOutbox outbox,
    ILogger<CreateIssueCommentHandler> logger
) : ICommandHandler<CreateIssueComment, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssueComment command, CancellationToken ct)
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);

        var audit = new IssueAudit
        {
            Action = IssueAuditAction.Comment,
            IssueId = command.IssueId,
            UserId = command.AuthorId,
            Data = JsonSerializer.SerializeToDocument(new { content = command.Content }).Deserialize<JsonDocument>(),
        };

        db.Add(audit);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property => (property, $"Invalid {property} reference"));
        }

        try
        {
            outbox.Enroll(db);
            await outbox
                .PublishAsync(new IssueCommentCreated { IssueId = audit.IssueId, IssueAuditId = audit.Id })
                .ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish IssueCommentCreated message");
            return Errors.Outbox();
        }

        return new Success();
    }
}
