using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserProfiles.Patch;

public sealed class PatchUserProfileHandler(AppDbContext db)
    : ICommandHandler<PatchUserProfile, OneOf<NotFoundError, InvalidPatchError, Success>>
{
    public async Task<OneOf<NotFoundError, InvalidPatchError, Success>> ExecuteAsync(
        PatchUserProfile command,
        CancellationToken ct
    )
    {
        Expression<Func<SetPropertyCalls<UserProfile>, SetPropertyCalls<UserProfile>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.DisplayName, out var displayName) && !string.IsNullOrEmpty(displayName))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.DisplayName, displayName));
        }
        if (command.Patch.TryGetValue(a => a.Bio, out var bio))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Bio, bio));
        }

        if (updateEx is null)
        {
            return new InvalidPatchError();
        }

        var count = await db
            .UserProfiles.Where(a => a.UserId == command.UserId)
            .ExecuteUpdateAsync(updateEx, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
