using System.Text.Json;
using NATS.Client.Core;
using WebApp.Common.IdEncoding;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Messaging.Common;

namespace WebApp.Infrastructure.Messaging;

public static class WorkspaceMemberInvitedHandler
{
    public static async Task HandleAsync(
        WorkspaceMemberInvitedUserNotified notified,
        INatsClient natsClient,
        ILogger logger,
        IIdEncoder idEncoder,
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
                        new WorkspaceMemberInvitedEvent(
                            notified.UserId.ToBase64String(),
                            notified.UserNotificationId.Value,
                            notified.Type,
                            idEncoder.Encode(notified.WorkspaceInvitationId.Value),
                            notified.WorkspacePath,
                            notified.WorkspaceName
                        ),
                        MessagingJsonContext.Default.WorkspaceMemberInvitedEvent
                    ),
                    headers: headers,
                    cancellationToken: ct
                )
                .ConfigureAwait(false);
        }
        catch (NatsException e)
        {
            logger.LogError(e, "Failed to publish WorkspaceMemberInvitedEvent to NATS");
            throw;
        }
    }
}
