using System.Text.Json;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class DefaultNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return false;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    ) { }

    public Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        return notification;
    }
}
