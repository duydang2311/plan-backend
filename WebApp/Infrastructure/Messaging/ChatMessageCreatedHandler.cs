using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Nats.Abstractions;

namespace WebApp.Infrastructure.Messaging;

public static class ChatMessageCreatedHandler
{
    public static async Task HandleAsync(
        ChatMessageCreated created,
        INatsConnectionFactory natsFactory,
        ILogger logger,
        IOptions<JsonOptions> options,
        CancellationToken ct
    )
    {
        try
        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            await using var nats = natsFactory.CreateNatsConnection();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            await nats.PublishAsync(
                    $"chats.{created.ChatId.ToBase64String()}.messages.created",
                    JsonSerializer.Serialize(
                        new { chatMessageId = created.ChatMessageId },
                        options.Value.SerializerOptions
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
