using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.Create;

public sealed record NotifyIssueCreatedJob : ICommand
{
    public required IssueId IssueId { get; init; }
    public required ProjectId ProjectId { get; init; }
}
