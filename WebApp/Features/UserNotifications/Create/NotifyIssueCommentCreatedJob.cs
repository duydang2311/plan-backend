using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.Create;

public sealed record NotifyIssueCommentCreatedJob : ICommand
{
    public required IssueId IssueId { get; init; }
    public required long IssueAuditId { get; init; }
}
