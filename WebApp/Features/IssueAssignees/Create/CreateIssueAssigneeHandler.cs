using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAssignees.Create;

using Result = OneOf<ValidationFailures, Success>;

public sealed record CreateIssueAssigneeHandler(AppDbContext db) : ICommandHandler<CreateIssueAssignee, Result>
{
    public async Task<Result> ExecuteAsync(CreateIssueAssignee command, CancellationToken ct)
    {
        var assignee = new IssueAssignee { IssueId = command.IssueId, UserId = command.UserId };
        db.Add(assignee);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            var failures = ValidationFailures.Many(2);
            foreach (var property in e.ConstraintProperties)
            {
                if (property.Equals(nameof(IssueAssignee.IssueId)))
                {
                    failures.Add("issue_id", "Issue does not exist", "invalid_reference");
                }
                else if (property.Equals(nameof(IssueAssignee.UserId)))
                {
                    failures.Add("user_id", "User does not exist", "invalid_reference");
                }
                else
                {
                    failures.Add(property, $"{property} does not exist", "invalid_reference");
                }
            }
            return failures;
        }
        catch (UniqueConstraintException e)
        {
            var failures = ValidationFailures.Many(2);
            foreach (var property in e.ConstraintProperties)
            {
                if (property.Equals(nameof(IssueAssignee.IssueId)))
                {
                    failures.Add("issue_id", "User is already assigned", "unique");
                }
                else if (property.Equals(nameof(IssueAssignee.UserId)))
                {
                    failures.Add("user_id", "User is already assigned", "unique");
                }
                else
                {
                    failures.Add(property, "User is already assigned", "unique");
                }
            }
            return failures;
        }

        return new Success();
    }
}
