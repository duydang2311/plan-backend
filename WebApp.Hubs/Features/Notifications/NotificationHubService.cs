using System.Text.Json;
using System.Text.Json.Serialization;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using NATS.Client.Core;
using WebApp.Domain.Constants;
using WebApp.Hubs.Features.Hubs;
using WebApp.Infrastructure.Messaging.Common;

namespace WebApp.Hubs.Features.Notifications;

public sealed class NotificationHubService(
    INatsClient natsClient,
    IHubContext<MainHub> mainHubContext,
    ILogger<NotificationHubService> logger
) : IHostedService, IHubService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = SubscribeToNotificationAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnConnectedAsync(HubCallerContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(HubCallerContext context, Exception? exception)
    {
        return Task.CompletedTask;
    }

    async Task SubscribeToNotificationAsync(CancellationToken ct)
    {
        logger.LogInformation("Subscribing to notification messages");
        await foreach (
            var msg in natsClient
                .SubscribeAsync<string>("users.notifications", cancellationToken: ct)
                .ConfigureAwait(false)
        )
        {
            if (msg.Error is not null)
            {
                logger.LogError(msg.Error, "Error while receiving notification message");
                continue;
            }

            if (msg.Data is null)
            {
                logger.LogError(msg.Error, "Message data is null");
                continue;
            }

            var notificationType = msg.Headers?["Notification-Type"].FirstOrDefault();
            if (notificationType is null)
            {
                logger.LogError("Notification-Type header is missing in the message");
                continue;
            }
            // TODO: strategy pattern
            switch (notificationType)
            {
                case nameof(NotificationType.ProjectCreated):
                {
                    var deserializeAttempt = Attempt(
                        () => JsonSerializer.Deserialize(msg.Data, MessagingJsonContext.Default.ProjectCreatedEvent)
                    );
                    if (deserializeAttempt.TryGetError(out var e, out var data))
                    {
                        logger.LogError(e, "Error while deserializing ProjectCreatedEvent message");
                        continue;
                    }
                    await mainHubContext
                        .Clients.User(data.UserId)
                        .SendAsync("new_notification", data, ct)
                        .ConfigureAwait(false);
                    break;
                }
                case nameof(NotificationType.IssueCreated):
                {
                    var deserializeAttempt = Attempt(
                        () => JsonSerializer.Deserialize(msg.Data, MessagingJsonContext.Default.IssueCreatedEvent)
                    );
                    if (deserializeAttempt.TryGetError(out var e, out var data))
                    {
                        logger.LogError(e, "Error while deserializing IssueCreatedEvent message");
                        continue;
                    }
                    await mainHubContext
                        .Clients.User(data.UserId)
                        .SendAsync("new_notification", data, ct)
                        .ConfigureAwait(false);
                    break;
                }
                case nameof(NotificationType.IssueCommentCreated):
                {
                    var deserializeAttempt = Attempt(
                        () =>
                            JsonSerializer.Deserialize(msg.Data, MessagingJsonContext.Default.IssueCommentCreatedEvent)
                    );
                    if (deserializeAttempt.TryGetError(out var e, out var data))
                    {
                        logger.LogError(e, "Error while deserializing IssueCommentCreatedEvent message");
                        continue;
                    }
                    await mainHubContext
                        .Clients.User(data.UserId)
                        .SendAsync("new_notification", data, ct)
                        .ConfigureAwait(false);
                    break;
                }
                case nameof(NotificationType.ProjectMemberInvited):
                {
                    var deserializeAttempt = Attempt(
                        () =>
                            JsonSerializer.Deserialize(msg.Data, MessagingJsonContext.Default.ProjectMemberInvitedEvent)
                    );
                    if (deserializeAttempt.TryGetError(out var e, out var data))
                    {
                        logger.LogError(e, "Error while deserializing ProjectMemberInvitedEvent message");
                        continue;
                    }
                    await mainHubContext
                        .Clients.User(data.UserId)
                        .SendAsync("new_notification", data, ct)
                        .ConfigureAwait(false);
                    break;
                }
                default:
                {
                    logger.LogWarning("Unknown notification type: {NotificationType}", notificationType);
                    continue;
                }
            }
        }
    }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ChatMessageCreatedEvent))]
[JsonSerializable(typeof(IssueCommentCreatedEvent))]
[JsonSerializable(typeof(IssueCreatedEvent))]
[JsonSerializable(typeof(ProjectCreatedEvent))]
[JsonSerializable(typeof(ProjectMemberInvitedEvent))]
internal partial class MessagingJsonContext : JsonSerializerContext;
