using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCommentCreated : ICommand
{
    public required IssueComment IssueComment { get; init; }
}
