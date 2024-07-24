using EntityFramework.Exceptions.Common;
using FastEndpoints;
using MassTransit.Mediator;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Create;

using Result = OneOf<ValidationFailures, IssueComment>;

public sealed class CreateIssueCommentHandler(AppDbContext dbContext, IScopedMediator mediator) : ICommandHandler<CreateIssueComment, Result>
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
        await mediator.Publish(new IssueCommentCreated { IssueComment = comment }, ct)
            .ConfigureAwait(false);
        try
        {
            await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException exception)
        {
            return ValidationFailures.Single(exception.ConstraintProperties[0], "Reference does not exist", "reference");
        }
        return comment;
    }
}
