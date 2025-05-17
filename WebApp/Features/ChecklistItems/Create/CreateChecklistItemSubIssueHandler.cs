using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.Create;

public sealed record CreateChecklistItemSubIssueHandler(AppDbContext db)
    : ICommandHandler<
        CreateChecklistItemSubIssue,
        OneOf<ParentIssueNotFoundError, SubIssueNotFoundError, DuplicatedError, ChecklistItem>
    >
{
    public async Task<
        OneOf<ParentIssueNotFoundError, SubIssueNotFoundError, DuplicatedError, ChecklistItem>
    > ExecuteAsync(CreateChecklistItemSubIssue command, CancellationToken ct)
    {
        var checklistItem = new ChecklistItem
        {
            ParentIssueId = command.ParentIssueId,
            Kind = ChecklistItemKind.SubIssue,
            SubIssueId = command.SubIssueId,
        };

        db.Add(checklistItem);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            foreach (var property in e.ConstraintProperties)
            {
                if (property.Equals(nameof(ChecklistItem.ParentIssueId), StringComparison.OrdinalIgnoreCase))
                {
                    return new ParentIssueNotFoundError();
                }
                if (property.Equals(nameof(ChecklistItem.SubIssueId), StringComparison.OrdinalIgnoreCase))
                {
                    return new SubIssueNotFoundError();
                }
            }
            throw;
        }
        catch (UniqueConstraintException e)
        {
            foreach (var property in e.ConstraintProperties)
            {
                if (
                    property.EqualsEither(
                        [nameof(ChecklistItem.ParentIssueId), nameof(ChecklistItem.SubIssueId)],
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    return new DuplicatedError();
                }
            }
            throw;
        }

        return checklistItem;
    }
}
