using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using NATS.Client.Core;
using WebApp.Infrastructure.Nats.Abstractions;

namespace WebApp.Hubs.Features.Chats;

public sealed class ChatBroadcastService(
    INatsConnectionFactory natsConnectionFactory,
    IHubContext<ChatHub> chatHubContext,
    ILogger<ChatBroadcastService> logger
) : IHostedService
{
    INatsConnection? natsConnection;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        natsConnection = natsConnectionFactory.CreateNatsConnection();
        _ = SubscribeToChatMessageCreatedAsync(natsConnection, cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (natsConnection is not null)
        {
            await natsConnection.DisposeAsync().ConfigureAwait(false);
        }
    }

    async Task SubscribeToChatMessageCreatedAsync(INatsConnection natsConnection, CancellationToken ct)
    {
        try
        {
            await foreach (
                var msg in natsConnection
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
