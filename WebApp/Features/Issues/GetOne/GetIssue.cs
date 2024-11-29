using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.GetOne;

public sealed record GetIssue : ICommand<OneOf<None, Issue>>
{
    public IssueId? IssueId { get; init; }
    public ProjectId? ProjectId { get; init; }
    public long? OrderNumber { get; init; }
    public string? Select { get; init; }
}
