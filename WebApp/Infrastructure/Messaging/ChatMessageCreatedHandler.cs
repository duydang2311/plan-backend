using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Infrastructure.Messaging;

public static class ChatMessageCreatedHandler
{
    public static async Task HandleAsync(
        ChatMessageCreated created,
        INatsClient natsClient,
        ILogger logger,
        IOptions<JsonOptions> options,
        CancellationToken ct
    )
    {
        try
        {
            await natsClient
                .PublishAsync(
                    $"chats.messages.created",
                    JsonSerializer.Serialize(
                        new Payload(created.ChatId.ToBase64String(), created.ChatMessageId.Value),
                        ChatMessageCreatedJsonContext.Default.Payload
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

    public sealed record Payload(string ChatId, long ChatMessageId) { }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(ChatMessageCreatedHandler.Payload))]
internal partial class ChatMessageCreatedJsonContext : JsonSerializerContext;
