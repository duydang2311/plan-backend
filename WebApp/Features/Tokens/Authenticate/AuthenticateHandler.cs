using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using WebApp.SharedKernel.Hashers.Abstractions;
using WebApp.SharedKernel.Jwts.Abstractions;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Tokens.Authenticate;

using Result = OneOf<IEnumerable<ValidationError>, AuthenticateResult>;

public sealed class AuthenticateHandler(
    IOptions<JwtOptions> options,
    IJwtService jwtService,
    IHasher hasher,
    AppDbContext dbContext
) : ICommandHandler<AuthenticateCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateCommand command, CancellationToken ct)
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
            return new[] { new ValidationError("email", "Email is not found", "email_not_found"), };
        }

        if (!hasher.Verify(command.Password, user.PasswordHash, user.Salt))
        {
            return new[]
            {
                new ValidationError("email", "Authentication credentials is invalid", "invalid_credentials"),
                new ValidationError("password", "Authentication credentials is invalid", "invalid_credentials")
            };
        }

        var o = options.Value;
        var refreshToken = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var accessTokenMaxAge = TimeSpan.FromMinutes(5);
        var accessToken = jwtService.CreateToken(
            issuer: o.ValidIssuers.FirstOrDefault(),
            audience: o.ValidAudiences.FirstOrDefault(),
            claims: [new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString("D"))],
            notBefore: now,
            expires: now.Add(accessTokenMaxAge),
            issuedAt: now
        );

        return new AuthenticateResult(
            jwtService.WriteToken(accessToken),
            refreshToken,
            (int)accessTokenMaxAge.TotalSeconds,
            (int)TimeSpan.FromDays(1).TotalSeconds
        );
    }
}
