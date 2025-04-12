using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using NATS.Client.Core;
using WebApp.Hubs.Common;
using WebApp.Hubs.Features.Hubs;

namespace WebApp.Hubs.Features.Chats;

public sealed class ChatHubService(
    INatsClient natsClient,
    IHubContext<MainHub> mainHubContext,
    IApiHttpClientFactory httpClientFactory,
    ILogger<ChatHubService> logger
) : IHostedService, IHubService
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

    public async Task OnConnectedAsync(HubCallerContext context)
    {
        if (string.IsNullOrEmpty(context.UserIdentifier))
        {
            return;
        }
        await AddUserToChatGroupsAsync(context.ConnectionId, context.UserIdentifier, context.ConnectionAborted)
            .ConfigureAwait(true);
    }

    public Task OnDisconnectedAsync(HubCallerContext context, Exception? exception)
    {
        return Task.CompletedTask;
    }

    async Task AddUserToChatGroupsAsync(string connectionId, string userId, CancellationToken ct)
    {
        using var httpClient = httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync($"get-user-chat-ids/v1?userId={userId}", ct).ConfigureAwait(true);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var chatIds = await response.Content.ReadFromJsonAsync<List<string>>(ct).ConfigureAwait(true);
            if (chatIds is null)
            {
                return;
            }

            foreach (var chatId in chatIds)
            {
                Console.WriteLine("Add to group " + ChatUtils.GroupName(chatId));
                _ = mainHubContext
                    .Groups.AddToGroupAsync(connectionId, ChatUtils.GroupName(chatId), ct)
                    .ConfigureAwait(true);
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to add user to chat groups due to HTTP request error");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while adding user to chat groups");
            throw;
        }
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
                await mainHubContext
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

public sealed record ChatMessageCreatedPayload(string ChatId, long ChatMessageId, string? OptimisticId) { }

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ChatMessageCreatedPayload))]
public partial class ChatMessageCreatedJsonContext : JsonSerializerContext { }
