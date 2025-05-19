using FastEndpoints;
using OneOf;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChecklistItems.Create;

public sealed record CreateChecklistItemTodo
    : ICommand<OneOf<ParentIssueNotFoundError, IReadOnlyCollection<ChecklistItem>>>
{
    public required IssueId ParentIssueId { get; init; }
    public required string[] Contents { get; init; }
}
