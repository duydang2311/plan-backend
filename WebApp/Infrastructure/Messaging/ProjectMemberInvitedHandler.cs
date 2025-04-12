using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Infrastructure.Messaging;

public static class ProjectMemberInvitedHandler
{
    public static async Task HandleAsync(
        ProjectMemberInvitedUserNotified notified,
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
                        new ProjectMemberInvitedPayload(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            notified.ProjectMemberInvitationId.Value,
                            notified.ProjectIdentifier,
                            notified.ProjectName
                        ),
                        MessagingJsonContext.Default.ProjectMemberInvitedPayload
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

public sealed record ProjectMemberInvitedPayload(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    long ProjectMemberInvitationId,
    string ProjectIdentifier,
    string ProjectName
) { }
