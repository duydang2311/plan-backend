using FastEndpoints;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Tokens.Authenticate.WithGoogle;

using Result = OneOf<ValidationFailures, AuthenticateResult>;

public sealed class AuthenticateWithGoogleHandler(AppDbContext db)
    : ICommandHandler<AuthenticateWithGoogleCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateWithGoogleCommand command, CancellationToken ct)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(command.IdToken).ConfigureAwait(false);
        }
        catch (InvalidJwtException)
        {
            return ValidationFailures.Single("idToken", "Invalid token", "invalid");
        }

        var user = await db
            .Users.Where(a => a.GoogleAuth != null && a.GoogleAuth.GoogleId.Equals(payload.Subject))
            .Select(a => new { a.Id })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (user is null)
        {
            return ValidationFailures.Single("user", "User has not been registered", "unregistered");
        }

        var session = new UserSession { Token = IdHelper.NewSessionId(), UserId = user.Id };
        db.Add(session);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        return new AuthenticateResult
        {
            SessionId = session.Token,
            SessionMaxAge = (int)TimeSpan.FromDays(45).TotalSeconds,
        };
    }
}
