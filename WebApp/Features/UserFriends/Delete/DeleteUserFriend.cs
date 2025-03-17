using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriends.Delete;

public sealed record DeleteUserFriend : ICommand<OneOf<NotFoundError, Success>>
{
    public required UserId UserId { get; init; }
    public required UserId FriendId { get; init; }
}
