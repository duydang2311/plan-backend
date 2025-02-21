using EntityFrameworkCore.Projectables;
using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record UserNotification
{
    public UserNotificationId Id { get; init; }
    public Instant CreatedTime { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public NotificationId NotificationId { get; init; }
    public Notification Notification { get; init; } = null!;
    public UserNotificationReadFlag? ReadFlag { get; init; }

    [Projectable(UseMemberBody = nameof(isRead))]
    public bool IsRead { get; init; }

    private bool isRead => ReadFlag != null;
}
