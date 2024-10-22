using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.UserSessions.GetOne;

public sealed record Response
{
    public Instant? CreatedTime { get; init; }
    public SessionToken? Token { get; init; }
    public UserId? UserId { get; init; }
    public User? User { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this UserSession userSession);
}
