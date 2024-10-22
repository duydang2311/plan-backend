using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Exceptions;
using WebApp.Domain.Commands;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserProfiles;

public sealed class UpdateUserProfileImageHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<UpdateUserProfileImage>
{
    public async Task ExecuteAsync(UpdateUserProfileImage command, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var count = await db
            .UserProfiles.Where(a => a.UserId == command.UserId)
            .ExecuteUpdateAsync(
                a =>
                    a.SetProperty(b => b.Image.PublicId, command.PublicId)
                        .SetProperty(b => b.Image.ResourceType, command.ResourceType)
                        .SetProperty(b => b.Image.Format, command.Format)
                        .SetProperty(b => b.Image.Version, command.Version),
                cancellationToken: ct
            )
            .ConfigureAwait(false);

        if (count == 0)
        {
            throw new JobFailureException("The user profile was not found.");
        }
    }
}
