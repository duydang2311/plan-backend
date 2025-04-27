using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Hashers.Abstractions;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Tokens.Authenticate.WithCredentials;

using Result = OneOf<ValidationFailures, AuthenticateResult>;

public sealed class AuthenticateWithCredentialsHandler(IHasher hasher, AppDbContext dbContext)
    : ICommandHandler<AuthenticateWithCredentialsCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateWithCredentialsCommand command, CancellationToken ct)
    {
        var user = await dbContext
            .Users.Where(a => EF.Functions.Collate(a.Email, "case_insensitive") == command.Email)
            .Select(a => new
            {
                a.Id,
                a.Salt,
                a.PasswordHash,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (user is null)
        {
            return ValidationFailures.Single("email", "Email is not found", "email_not_found");
        }

        if (
            user.PasswordHash is null
            || user.Salt is null
            || !hasher.Verify(command.Password, user.PasswordHash, user.Salt)
        )
        {
            return ValidationFailures
                .Many(2)
                .Add("email", "Authentication credentials is invalid", "invalid_credentials")
                .Add("password", "Authentication credentials is invalid", "invalid_credentials");
        }

        var userRefreshToken = new UserRefreshToken { UserId = user.Id };
        var session = new UserSession { SessionId = IdHelper.NewSessionId(), UserId = user.Id };

        dbContext.Add(session);
        dbContext.Add(userRefreshToken);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        return new AuthenticateResult
        {
            SessionId = session.SessionId,
            SessionMaxAge = (int)TimeSpan.FromDays(45).TotalSeconds,
        };
    }
}
