using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs.Features.Hubs;

[Authorize]
public sealed class MainHub(IEnumerable<IHubService> hubServices) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier))
        {
            await base.OnConnectedAsync().ConfigureAwait(true);
            return;
        }

        foreach (var hubService in hubServices)
        {
            await hubService.OnConnectedAsync(Context).ConfigureAwait(true);
        }
        await base.OnConnectedAsync().ConfigureAwait(true);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var hubService in hubServices)
        {
            await hubService.OnDisconnectedAsync(Context, exception).ConfigureAwait(true);
        }
        await base.OnConnectedAsync().ConfigureAwait(true);
    }
}
