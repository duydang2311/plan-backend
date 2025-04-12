using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public abstract record UserNotified
{
    public required UserId UserId { get; init; }
    public required UserNotificationId UserNotificationId { get; init; }
    public required NotificationType Type { get; init; }
}
