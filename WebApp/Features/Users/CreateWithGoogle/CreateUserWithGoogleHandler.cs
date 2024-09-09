using FastEndpoints;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.CreateWithGoogle;

using Result = OneOf<ValidationFailures, User>;

public sealed class CreateUserWithGoogleHandler(AppDbContext dbContext)
    : ICommandHandler<CreateUserWithGoogleCommand, Result>
{
    public async Task<Result> ExecuteAsync(CreateUserWithGoogleCommand command, CancellationToken ct)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken).ConfigureAwait(false);
        }
        catch (InvalidJwtException)
        {
            return ValidationFailures.Single("idToken", "Invalid ID token", "invalid");
        }

        if (
            await dbContext
                .Users.AnyAsync(x => EF.Functions.Collate(x.Email, "case_insensitive") == payload.Email, ct)
                .ConfigureAwait(false)
        )
        {
            return ValidationFailures.Single("email", "Email has already been used", "duplicated_email");
        }

        var user = new User { Email = payload.Email, IsVerified = true };
        var googleAuth = new UserGoogleAuth
        {
            User = user,
            GoogleId = payload.Subject,
            Email = payload.Email,
        };

        dbContext.Add(user);
        dbContext.Add(googleAuth);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return user with { GoogleAuth = googleAuth };
    }
}
