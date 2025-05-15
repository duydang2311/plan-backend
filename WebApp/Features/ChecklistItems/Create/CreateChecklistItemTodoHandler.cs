using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.Create;

public sealed record CreateChecklistItemTodoHandler(AppDbContext db)
    : ICommandHandler<CreateChecklistItemTodo, OneOf<ParentIssueNotFoundError, ChecklistItem>>
{
    public async Task<OneOf<ParentIssueNotFoundError, ChecklistItem>> ExecuteAsync(
        CreateChecklistItemTodo command,
        CancellationToken ct
    )
    {
        var checklistItem = new ChecklistItem
        {
            ParentIssueId = command.ParentIssueId,
            Kind = ChecklistItemKind.Todo,
            Content = command.Content,
            Completed = false,
        };

        db.Add(checklistItem);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            if (
                e.ConstraintProperties.Any(a =>
                    a.Equals(nameof(ChecklistItem.ParentIssueId), StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return new ParentIssueNotFoundError();
            }
            throw;
        }

        return checklistItem;
    }
}
