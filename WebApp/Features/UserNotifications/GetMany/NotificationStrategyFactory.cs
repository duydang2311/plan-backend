using System.Collections.Concurrent;
using WebApp.Domain.Constants;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class NotificationStrategyFactory(params IEnumerable<INotificationProcessingStrategy> strategies)
{
    static readonly INotificationProcessingStrategy defaultStrategy = new DefaultNotificationStrategy();

    readonly ConcurrentDictionary<NotificationType, INotificationProcessingStrategy> strategiesMap = [];

    public INotificationProcessingStrategy GetStrategy(NotificationType type)
    {
        return strategiesMap.GetOrAdd(
            type,
            (type) => strategies.FirstOrDefault(a => a.CanProcess(type)) ?? defaultStrategy
        );
    }
}
