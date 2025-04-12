using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Infrastructure.Messaging;

public static class IssueCommentCreatedHandler
{
    public static async Task HandleAsync(
        IssueCommentCreatedUserNotified notified,
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
                        new IssueCommentCreatedPayload(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            notified.OrderNumber,
                            notified.Title,
                            notified.ProjectIdentifier,
                            notified.WorkspacePath
                        ),
                        MessagingJsonContext.Default.IssueCreatedPayload
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

public sealed record IssueCommentCreatedPayload(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    long OrderNumber,
    string Title,
    string ProjectIdentifier,
    string WorkspacePath
) { }
