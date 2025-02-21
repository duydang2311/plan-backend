using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record UserNotificationReadFlag
{
    public Instant CreatedTime { get; init; }
    public UserNotificationId UserNotificationId { get; init; }
    public UserNotification UserNotification { get; init; } = null!;
}
