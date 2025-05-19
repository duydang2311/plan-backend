using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.Create;

public sealed record CreateChecklistItemTodoHandler(AppDbContext db)
    : ICommandHandler<CreateChecklistItemTodo, OneOf<ParentIssueNotFoundError, IReadOnlyCollection<ChecklistItem>>>
{
    public async Task<OneOf<ParentIssueNotFoundError, IReadOnlyCollection<ChecklistItem>>> ExecuteAsync(
        CreateChecklistItemTodo command,
        CancellationToken ct
    )
    {
        var checklistItems = command
            .Contents.Select(a => new ChecklistItem
            {
                ParentIssueId = command.ParentIssueId,
                Kind = ChecklistItemKind.Todo,
                Content = a,
                Completed = false,
            })
            .ToList();

        db.AddRange(checklistItems);
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

        return checklistItems;
    }
}
