using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCommentCreated : IEvent
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required IssueComment IssueComment { get; init; }
}
