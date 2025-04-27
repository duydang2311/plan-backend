using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.UserSessions.GetOne.ByToken;

namespace WebApp.Api.V1.UserSessions.GetOne.ByToken;

public sealed record Request
{
    public SessionId Token { get; init; }
    public string? Select { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetUserSessionByToken ToCommand(this Request request);
}
