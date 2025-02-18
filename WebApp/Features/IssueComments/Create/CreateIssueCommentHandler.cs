using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Create;

using Result = OneOf<ValidationFailures, IssueComment>;

public sealed class CreateIssueCommentHandler(AppDbContext dbContext) : ICommandHandler<CreateIssueComment, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssueComment command, CancellationToken ct)
    {
        var comment = new IssueComment
        {
            IssueId = command.IssueId,
            AuthorId = command.AuthorId,
            Content = command.Content,
        };
        dbContext.Add(comment);
        dbContext.Add(
            new IssueAudit
            {
                Action = IssueAuditAction.Comment,
                IssueId = command.IssueId,
                UserId = command.AuthorId,
                Data = JsonValue.Create(new CommentData { Content = command.Content }).Deserialize<JsonDocument>(),
            }
        );
        try
        {
            await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property => (property, $"Invalid {property} reference"));
        }
        await new IssueCommentCreated { IssueComment = comment }
            .QueueJobAsync(ct: ct)
            .ConfigureAwait(false);
        return comment;
    }

    public record CommentData
    {
        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;
    }
}
