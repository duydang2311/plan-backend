using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Messaging.Common;

namespace WebApp.Infrastructure.Messaging;

public static class IssueCreatedHandler
{
    public static async Task HandleAsync(
        IssueCreatedUserNotified notified,
        INatsClient natsClient,
        ILogger logger,
        CancellationToken ct
    )
    {
        try
        {
            var headers = new NatsHeaders(1) { { "Notification-Type", notified.Type.ToString() } };
            await natsClient
                .PublishAsync(
                    "users.notifications",
                    JsonSerializer.Serialize(
                        new IssueCreatedEvent(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            notified.OrderNumber,
                            notified.Title,
                            notified.ProjectIdentifier,
                            notified.WorkspacePath
                        ),
                        MessagingJsonContext.Default.IssueCreatedEvent
                    ),
                    headers: headers,
                    cancellationToken: ct
                )
                .ConfigureAwait(false);
        }
        catch (NatsException e)
        {
            logger.LogError(e, "Failed to publish chat message created event to NATS");
            throw;
        }
    }
}
