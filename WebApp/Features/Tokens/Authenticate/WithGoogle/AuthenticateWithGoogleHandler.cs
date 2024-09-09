using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using Google.Apis.Auth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using WebApp.Common.Jwts.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Tokens.Authenticate.WithGoogle;

using Result = OneOf<ValidationFailures, AuthenticateResult>;

public sealed class AuthenticateWithGoogleHandler(IOptions<JwtOptions> options, IJwtService jwtService, AppDbContext db)
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

        var userRefreshToken = new UserRefreshToken { UserId = user.Id };
        db.Add(userRefreshToken);

        var o = options.Value;
        var task = db.SaveChangesAsync(ct);
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

        return new AuthenticateResult(
            jwtService.WriteToken(accessToken),
            userRefreshToken.Token,
            (int)accessTokenMaxAge.TotalSeconds,
            (int)TimeSpan.FromDays(1).TotalSeconds
        );
    }
}
