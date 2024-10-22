using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserSessions.GetOne.ByToken;

public sealed record GetUserSessionByToken : ICommand<OneOf<NotFoundError, UserSession>>
{
    public required SessionToken Token { get; init; }
    public string? Select { get; init; }
}
