using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriends.Create;

public sealed record CreateUserFriend : ICommand<OneOf<ValidationFailures, DuplicatedError, Success>>
{
    public required UserId UserId { get; init; }
    public required UserId FriendId { get; init; }
}
