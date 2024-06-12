using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using Microsoft.Extensions.Options;
using NanoidDotNet;
using OneOf;
using WebApp.SharedKernel.Jwts.Abstractions;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Tokens.Authenticate;

using Result = OneOf<IEnumerable<ValidationError>, AuthenticateResult>;

public sealed class AuthenticateHandler(IOptions<JwtOptions> options, IJwtService jwtService)
    : ICommandHandler<AuthenticateCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateCommand command, CancellationToken ct)
    {
        if (command is not { Email: "test@gmail.com", Password: "123456" })
        {
            return new[]
            {
                new ValidationError("email", "Authentication credentials is invalid", "invalid_credentials"),
                new ValidationError("password", "Authentication credentials is invalid", "invalid_credentials")
            };
        }

        var o = options.Value;
        var generateRefreshTokenTask = Nanoid.GenerateAsync();
        var now = DateTime.UtcNow;
        var accessTokenMaxAge = TimeSpan.FromMinutes(5);
        var accessToken = jwtService.CreateToken(
            issuer: o.ValidIssuers.FirstOrDefault(),
            audience: o.ValidAudiences.FirstOrDefault(),
            claims: [new Claim(JwtRegisteredClaimNames.Sub, command.Email)],
            notBefore: now,
            expires: now.Add(accessTokenMaxAge),
            issuedAt: now
        );

        return new AuthenticateResult(
            jwtService.WriteToken(accessToken),
            await generateRefreshTokenTask.ConfigureAwait(false),
            (int)accessTokenMaxAge.TotalSeconds,
            (int)TimeSpan.FromDays(1).TotalSeconds
        );
    }
}
