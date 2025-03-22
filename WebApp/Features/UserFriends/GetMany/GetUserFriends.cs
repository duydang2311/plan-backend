using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriends.GetMany;

public sealed record GetUserFriends : Collective, ICommand<PaginatedList<UserFriend>>
{
    public UserId? UserId { get; init; }
    public UserId? FriendId { get; init; }
    public string? Select { get; init; }
    public string? Query { get; init; }
}
