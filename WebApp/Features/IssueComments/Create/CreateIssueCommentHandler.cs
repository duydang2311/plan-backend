using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.Create;

using Result = OneOf<ValidationFailures, IssueComment>;

public sealed class CreateIssueCommentHandler(IServiceProvider serviceProvider, AppDbContext dbContext)
    : ICommandHandler<CreateIssueComment, Result>
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
        try
        {
            await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException exception)
        {
            return ValidationFailures.Single(
                exception.ConstraintProperties[0],
                "Reference does not exist",
                "reference"
            );
        }
        await new IssueCommentCreated { ServiceProvider = serviceProvider, IssueComment = comment }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(false);
        return comment;
    }
}
