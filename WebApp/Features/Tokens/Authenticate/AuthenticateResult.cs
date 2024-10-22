using WebApp.Domain.Entities;

namespace WebApp.Features.Tokens.Authenticate;

public sealed record AuthenticateResult
{
    public required string AccessToken { get; init; }
    public required RefreshToken RefreshToken { get; init; }
    public required int AccessTokenMaxAge { get; init; }
    public required int RefreshTokenMaxAge { get; init; }
    public required SessionToken SessionId { get; init; }
    public required int SessionMaxAge { get; init; }
}
