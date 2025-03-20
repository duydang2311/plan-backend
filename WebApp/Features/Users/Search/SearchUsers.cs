using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.Search;

public sealed record SearchUsers : Collective, ICommand<PaginatedList<User>>
{
    public required string Query { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
    public UserId? ExcludeFriendsWithUserId { get; init; }
    public UserId? ExcludeFriendRequestedWithUserId { get; init; }
}
