using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriendRequests.Delete;

namespace WebApp.Api.V1.UserFriendRequests.Delete;

public sealed record Request
{
    public UserId SenderId { get; init; }
    public UserId ReceiverId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial DeleteUserFriendRequest ToCommand(this Request request);
}
