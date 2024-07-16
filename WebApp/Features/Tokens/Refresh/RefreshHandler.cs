using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using WebApp.Common.Jwts.Abstractions;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Tokens.Refresh;

using Result = OneOf<IEnumerable<ValidationError>, RefreshResult>;

public sealed class RefreshHandler(IOptions<JwtOptions> options, IJwtService jwtService, AppDbContext dbContext)
    : ICommandHandler<RefreshCommand, Result>
{
    public async Task<Result> ExecuteAsync(RefreshCommand command, CancellationToken ct)
    {
        var user = await dbContext
            .UserRefreshTokens.Where(x => x.Token == command.Token)
            .Select(x => new { Id = x.UserId, })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (user is null)
        {
            return new[] { new ValidationError("token", "Token is invalid", "invalid"), };
        }

        var o = options.Value;
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

        return new RefreshResult(jwtService.WriteToken(accessToken), (int)accessTokenMaxAge.TotalSeconds);
    }
}
