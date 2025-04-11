using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApp.Hubs.Common;

namespace WebApp.Hubs.Features.Chats;

[Authorize]
public sealed class ChatHub(IApiHttpClientFactory httpClientFactory, ILogger<ChatHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier))
        {
            await base.OnConnectedAsync().ConfigureAwait(true);
            return;
        }

        await AddUserToChatGroupsAsync(Context.ConnectionId, Context.UserIdentifier, Context.ConnectionAborted)
            .ConfigureAwait(true);
        await base.OnConnectedAsync().ConfigureAwait(true);
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
                _ = Groups.AddToGroupAsync(connectionId, ChatUtils.GroupName(chatId), ct).ConfigureAwait(true);
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
}
