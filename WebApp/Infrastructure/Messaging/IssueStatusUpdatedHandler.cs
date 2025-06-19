using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Messaging.Common;

namespace WebApp.Infrastructure.Messaging;

public static class IssueStatusUpdatedHandler
{
    public static async Task HandleAsync(
        IssueStatusUpdatedUserNotified notified,
        INatsClient natsClient,
        ILogger logger,
        CancellationToken ct
    )
    {
        try
        {
            var headers = new NatsHeaders(1) { { "Notification-Type", notified.Type.ToString("D") } };
            await natsClient
                .PublishAsync(
                    "users.notifications",
                    JsonSerializer.Serialize(
                        new IssueStatusUpdatedEvent(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            notified.OrderNumber,
                            notified.Title,
                            notified.ProjectIdentifier,
                            notified.WorkspacePath,
                            notified.OldStatusCategory,
                            notified.OldStatusColor,
                            notified.OldStatusValue,
                            notified.NewStatusCategory,
                            notified.NewStatusColor,
                            notified.NewStatusValue
                        ),
                        MessagingJsonContext.Default.IssueStatusUpdatedEvent
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
