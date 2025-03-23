using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriends.Search;

public sealed record SearchUserFriends : Collective, ICommand<PaginatedList<UserFriend>>
{
    public required UserId UserId { get; init; }
    public required string Query { get; init; }
    public string? Select { get; init; }
}
