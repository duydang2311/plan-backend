using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using NATS.Client.Core;

namespace WebApp.Hubs.Features.Chats;

public sealed class ChatBroadcastService(
    INatsClient natsClient,
    IHubContext<ChatHub> chatHubContext,
    ILogger<ChatBroadcastService> logger
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = SubscribeToChatMessageCreatedAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    async Task SubscribeToChatMessageCreatedAsync(CancellationToken ct)
    {
        logger.LogInformation("Subscribing to chat message created events");
        try
        {
            await foreach (
                var msg in natsClient
                    .SubscribeAsync(
                        "chats.messages.created",
                        serializer: new NatsJsonContextSerializer<ChatMessageCreatedPayload>(
                            ChatMessageCreatedJsonContext.Default
                        ),
                        cancellationToken: ct
                    )
                    .ConfigureAwait(false)
            )
            {
                if (msg.Error is not null)
                {
                    logger.LogError(msg.Error, "Failed to deserialize chat message created event");
                    continue;
                }
                if (msg.Data is null)
                {
                    continue;
                }
                await chatHubContext
                    .Clients.Group(ChatUtils.GroupName(msg.Data.ChatId))
                    .SendAsync("new_chat_message", msg.Data, ct)
                    .ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while subscribing to chat message created events");
        }
    }
}

public sealed record ChatMessageCreatedPayload(string ChatId, long ChatMessageId) { }

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ChatMessageCreatedPayload))]
public partial class ChatMessageCreatedJsonContext : JsonSerializerContext { }
