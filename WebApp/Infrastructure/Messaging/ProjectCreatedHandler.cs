using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Infrastructure.Messaging;

public static class ProjectCreatedHandler
{
    public static async Task HandleAsync(
        ProjectCreatedUserNotified notified,
        INatsClient natsClient,
        ILogger logger,
        CancellationToken ct
    )
    {
        try
        {
            await natsClient
                .PublishAsync(
                    $"users.notifications",
                    JsonSerializer.Serialize(
                        new ProjectCreatedPayload(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            notified.Identifier,
                            notified.Name,
                            notified.WorkspacePath
                        ),
                        MessagingJsonContext.Default.ProjectCreatedPayload
                    ),
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

public sealed record ProjectCreatedPayload(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    string Identifier,
    string Name,
    string WorkspacePath
) { }
