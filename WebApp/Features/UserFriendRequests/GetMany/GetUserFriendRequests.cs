using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserFriendRequests.GetMany;

public sealed record GetUserFriendRequests : Collective, ICommand<PaginatedList<UserFriendRequest>>
{
    public UserId? SenderId { get; init; }
    public UserId? ReceiverId { get; init; }
    public string? Select { get; init; }
}
