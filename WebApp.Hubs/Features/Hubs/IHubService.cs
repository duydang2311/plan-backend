using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs.Features.Hubs;

public interface IHubService
{
    Task OnConnectedAsync(HubCallerContext context);
    Task OnDisconnectedAsync(HubCallerContext context, Exception? exception);
}
