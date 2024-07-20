using FastEndpoints;
using OneOf.Types;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.GetOne;

public sealed record GetIssue : Collective, ICommand<OneOf<None, Issue>>
{
    public IssueId? IssueId { get; init; }
    public TeamId? TeamId { get; init; }
    public long? OrderNumber { get; init; }
    public string? Select { get; init; }
}
