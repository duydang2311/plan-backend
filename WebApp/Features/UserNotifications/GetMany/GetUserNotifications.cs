using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed record GetUserNotifications : Collective, ICommand<PaginatedList<UserNotification>>
{
    public required UserId UserId { get; init; }
    public string? Select { get; init; }
    public string? SelectProject { get; init; }
    public string? SelectIssue { get; init; }
    public string? SelectComment { get; init; }
    public string? SelectProjectMemberInvitation { get; init; }
}
