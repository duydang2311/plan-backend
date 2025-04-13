using System.Text.Json;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Messaging.Common;

namespace WebApp.Infrastructure.Messaging;

public static class ChatMessageCreatedHandler
{
    public static async Task HandleAsync(
        ChatMessageCreated created,
        INatsClient natsClient,
        ILogger logger,
        CancellationToken ct
    )
    {
        try
        {
            await natsClient
                .PublishAsync(
                    "chats.messages.created",
                    JsonSerializer.Serialize(
                        new ChatMessageCreatedEvent(
                            created.ChatId.ToBase64String(),
                            created.ChatMessageId.Value,
                            created.OptimisticId
                        ),
                        MessagingJsonContext.Default.ChatMessageCreatedEvent
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
