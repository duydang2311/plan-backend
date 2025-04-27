using WebApp.Domain.Entities;

namespace WebApp.Features.Tokens.Authenticate;

public sealed record AuthenticateResult
{
    public required SessionId SessionId { get; init; }
    public required int SessionMaxAge { get; init; }
}
