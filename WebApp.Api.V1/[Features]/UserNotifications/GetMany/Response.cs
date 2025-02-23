using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.UserNotifications.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public UserNotificationId? Id { get; init; }
        public Instant? CreatedTime { get; init; }
        public UserId? UserId { get; init; }
        public NotificationId? NotificationId { get; init; }
        public Notification? Notification { get; init; }
        public bool? IsRead { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<UserNotification> list);

    public static partial Response.Item ToResponse(this UserNotification userNotification);
}
