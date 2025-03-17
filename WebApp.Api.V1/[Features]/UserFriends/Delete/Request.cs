using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriends.Delete;

namespace WebApp.Api.V1.UserFriends.Delete;

public sealed record Request
{
    public UserId UserId { get; init; }
    public UserId FriendId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial DeleteUserFriend ToCommand(this Request request);
}
