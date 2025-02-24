using System.Text.Json;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.Create;

public sealed record CreateUserNotification : ICommand<OneOf<InvalidUserError, Success>>
{
    public required UserId UserId { get; init; }
    public required NotificationType Type { get; init; }
    public JsonDocument? Data { get; init; }
}
