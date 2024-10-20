using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserProfiles;

public sealed class UserProfileImageUploadedHandler(IServiceScopeFactory scopeFactory)
    : IEventHandler<UserProfileImageUploaded>
{
    public async Task HandleAsync(UserProfileImageUploaded eventModel, CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db
            .UserProfiles.Where(a => a.UserId == eventModel.UserId)
            .ExecuteUpdateAsync(
                a =>
                    a.SetProperty(b => b.Image.PublicId, eventModel.PublicId)
                        .SetProperty(b => b.Image.ResourceType, eventModel.ResourceType)
                        .SetProperty(b => b.Image.Format, eventModel.Format)
                        .SetProperty(b => b.Image.Version, eventModel.Version),
                cancellationToken: ct
            )
            .ConfigureAwait(false);
    }
}
