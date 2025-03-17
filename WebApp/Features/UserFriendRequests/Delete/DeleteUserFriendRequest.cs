using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriendRequests.Delete;

public sealed record DeleteUserFriendRequest : ICommand<OneOf<NotFoundError, Success>>
{
    public required UserId SenderId { get; init; }
    public required UserId ReceiverId { get; init; }
}
