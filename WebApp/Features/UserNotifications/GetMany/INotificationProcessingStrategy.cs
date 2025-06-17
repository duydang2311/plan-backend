using System.Text.Json;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public interface INotificationProcessingStrategy
{
    bool CanProcess(NotificationType type);
    void CollectIds(UserNotification notification, GetUserNotifications command, NotificationCollectIdContext context);
    Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken);
    UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    );
}
