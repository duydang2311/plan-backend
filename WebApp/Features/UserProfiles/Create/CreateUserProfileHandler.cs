using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserProfiles.Create;

using Result = OneOf<NotFoundError, ValidationFailures, Success>;

public sealed class CreateUserProfileHandler(AppDbContext db) : ICommandHandler<CreateUserProfile, Result>
{
    public async Task<Result> ExecuteAsync(CreateUserProfile command, CancellationToken ct)
    {
        var profile = new UserProfile
        {
            UserId = command.UserId,
            Name = command.Name,
            DisplayName = command.DisplayName,
            Bio = command.Bio,
            Image = command.Image ?? Asset.Empty,
        };

        if (command.SocialLinks is not null)
        {
            profile = profile with { SocialLinks = [] };
            foreach (var socialLink in command.SocialLinks)
            {
                profile.SocialLinks.Add(new UserSocialLink { Profile = profile, Url = socialLink });
            }
        }

        db.Add(profile);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
