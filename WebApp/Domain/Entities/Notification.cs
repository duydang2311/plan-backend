using System.Text.Json;
using NodaTime;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record Notification
{
    public Instant CreatedTime { get; init; }
    public NotificationId Id { get; init; }
    public NotificationType Type { get; init; }
    public JsonDocument? Data { get; init; }
}
