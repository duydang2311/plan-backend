using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChecklistItems.Create;

public sealed record CreateChecklistItemSubIssue
    : ICommand<OneOf<ParentIssueNotFoundError, SubIssueNotFoundError, DuplicatedError, ChecklistItem>>
{
    public required IssueId ParentIssueId { get; init; }
    public required IssueId SubIssueId { get; init; }
}
