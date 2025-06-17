using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class NotificationLoadContext(
    NotificationCollectIdContext collectIdContext,
    NotificationHydrateContext hydrateContext,
    GetUserNotifications command,
    AppDbContext db
)
{
    private readonly HashSet<Type> loadedEntityTypes = [];

    public NotificationCollectIdContext CollectIdContext { get; } = collectIdContext;
    public NotificationHydrateContext HydrateContext { get; } = hydrateContext;
    public GetUserNotifications Command { get; } = command;
    public AppDbContext Db { get; } = db;

    public bool HasLoaded<TEntity>() => loadedEntityTypes.Contains(typeof(TEntity));

    public void MarkAsLoaded<TEntity>() => loadedEntityTypes.Add(typeof(TEntity));
}
