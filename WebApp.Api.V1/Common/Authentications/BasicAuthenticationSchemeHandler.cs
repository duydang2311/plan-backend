using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebApp.Api.V1.Common.Converters;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Common.Authentications;

public class BasicAuthenticationSchemeHandler(
    IOptionsMonitor<BasicAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AppDbContext db,
    IMemoryCache cache
) : AuthenticationHandler<BasicAuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string MissingHeader =
        $"Missing header '{BasicAuthenticationSchemeOptions.AuthorizationHeaderName}'.";
    private const string EmptyHeader = $"Header '{BasicAuthenticationSchemeOptions.AuthorizationHeaderName}' is empty.";
    private const string InvalidScheme = "Invalid authorization scheme";
    private const string MissingValue = "Missing session token";
    private const string BadToken = "Bad session token";
    private const string InvalidSession = "Invalid session";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            return AuthenticateResult.NoResult();
        }

        if (!Request.Headers.TryGetValue(BasicAuthenticationSchemeOptions.AuthorizationHeaderName, out var headerValue))
        {
            return AuthenticateResult.Fail(MissingHeader);
        }

        var header = headerValue.ToString();
        if (string.IsNullOrEmpty(header))
        {
            return AuthenticateResult.Fail(EmptyHeader);
        }

        var type = header[..Scheme.Name.Length];
        if (string.IsNullOrEmpty(type) || !type.Equals(Scheme.Name))
        {
            return AuthenticateResult.Fail(InvalidScheme);
        }

        var id = header[(Scheme.Name.Length + 1)..];
        if (string.IsNullOrEmpty(id))
        {
            return AuthenticateResult.Fail(MissingValue);
        }

        var parseResult = EntityGuidJsonConverter<SessionToken>.ValueParser(id);
        if (!parseResult.IsSuccess || parseResult.Value is null)
        {
            return AuthenticateResult.Fail(BadToken);
        }

        if (!cache.TryGetValue(parseResult.Value, out UserId userId))
        {
            userId = await db
                .UserSessions.Where(a => a.Token == (SessionToken)parseResult.Value)
                .Select(a => a.UserId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            if (userId == UserId.Empty)
            {
                return AuthenticateResult.Fail(InvalidSession);
            }

            cache.Set(
                parseResult.Value,
                userId,
                new MemoryCacheEntryOptions
                {
                    Size = 1,
                    SlidingExpiration = TimeSpan.FromMinutes(30),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                }
            );
        }

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }
}
