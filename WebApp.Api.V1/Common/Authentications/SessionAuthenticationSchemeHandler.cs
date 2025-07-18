using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using WebApp.Api.V1.Common.Converters;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Common.Authentications;

public class SessionAuthenticationSchemeHandler(
    IOptionsMonitor<SessionAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AppDbContext db,
    HybridCache cache
) : AuthenticationHandler<SessionAuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string MissingValue = "Missing session token";
    private const string BadToken = "Bad session token";
    private const string InvalidSession = "Invalid session";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            return AuthenticateResult.NoResult();
        }

        if (
            !Request.Headers.TryGetValue(
                SessionAuthenticationSchemeOptions.AuthorizationHeaderName,
                out var headerValue
            )
        )
        {
            return AuthenticateResult.NoResult();
        }

        var header = headerValue.ToString();
        if (string.IsNullOrEmpty(header))
        {
            return AuthenticateResult.NoResult();
        }

        var type = header[..Scheme.Name.Length];
        if (string.IsNullOrEmpty(type) || !type.Equals(Scheme.Name))
        {
            return AuthenticateResult.NoResult();
        }

        var id = header[(Scheme.Name.Length + 1)..];
        if (string.IsNullOrEmpty(id))
        {
            return AuthenticateResult.Fail(MissingValue);
        }

        var parseResult = EntityIdValueParsers.ParseString(id, value => new SessionId { Value = value });
        if (!parseResult.IsSuccess || parseResult.Value is null)
        {
            return AuthenticateResult.Fail(BadToken);
        }

        var value = (SessionId)parseResult.Value;

        var userId = await cache
            .GetOrCreateAsync(
                $"session-{value}",
                (token: value, db),
                static async (state, token) =>
                {
                    return await state
                        .db.UserSessions.Where(a => a.SessionId == state.token)
                        .Select(a => a.UserId)
                        .FirstOrDefaultAsync(CancellationToken.None)
                        .ConfigureAwait(false);
                }
            )
            .ConfigureAwait(false);

        if (userId == UserId.Empty)
        {
            await cache.RemoveAsync($"session-{parseResult.Value}").ConfigureAwait(false);
            return AuthenticateResult.Fail(InvalidSession);
        }

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }
}
