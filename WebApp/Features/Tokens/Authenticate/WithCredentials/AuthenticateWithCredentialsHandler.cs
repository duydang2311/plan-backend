using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using WebApp.Common.Hashers.Abstractions;
using WebApp.Common.Helpers;
using WebApp.Common.Jwts.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Tokens.Authenticate.WithCredentials;

using Result = OneOf<ValidationFailures, AuthenticateResult>;

public sealed class AuthenticateWithCredentialsHandler(
    IOptions<JwtOptions> options,
    IJwtService jwtService,
    IHasher hasher,
    AppDbContext dbContext
) : ICommandHandler<AuthenticateWithCredentialsCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateWithCredentialsCommand command, CancellationToken ct)
    {
        var user = await dbContext
            .Users.Where(a => EF.Functions.Collate(a.Email, "case_insensitive") == command.Email)
            .Select(a => new
            {
                a.Id,
                a.Salt,
                a.PasswordHash
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
        var session = new UserSession { Token = IdHelper.NewSessionId(), UserId = user.Id };

        dbContext.Add(session);
        dbContext.Add(userRefreshToken);

        var task = dbContext.SaveChangesAsync(ct);

        var o = options.Value;
        var now = DateTime.UtcNow;
        var accessTokenMaxAge = TimeSpan.FromMinutes(5);
        var accessToken = jwtService.CreateToken(
            issuer: o.ValidIssuers.FirstOrDefault(),
            audience: o.ValidAudiences.FirstOrDefault(),
            claims: [new Claim(JwtRegisteredClaimNames.Sub, Base64UrlTextEncoder.Encode(user.Id.Value.ToByteArray()))],
            notBefore: now,
            expires: now.Add(accessTokenMaxAge),
            issuedAt: now
        );

        await task.ConfigureAwait(false);

        return new AuthenticateResult
        {
            AccessToken = jwtService.WriteToken(accessToken),
            RefreshToken = userRefreshToken.Token,
            AccessTokenMaxAge = (int)accessTokenMaxAge.TotalSeconds,
            RefreshTokenMaxAge = (int)TimeSpan.FromDays(1).TotalSeconds,
            SessionId = session.Token,
            SessionMaxAge = (int)TimeSpan.FromDays(45).TotalSeconds,
        };
    }
}
