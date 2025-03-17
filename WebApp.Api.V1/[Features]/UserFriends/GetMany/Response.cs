using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.UserFriends.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant? CreatedTime { get; init; }
        public UserId? UserId { get; init; }
        public UserDto? User { get; init; }
        public UserId? FriendId { get; init; }
        public UserDto? Friend { get; init; }
    }

    public sealed record UserDto
    {
        public Instant? CreatedTime { get; init; }
        public Instant? UpdatedTime { get; init; }
        public UserId? Id { get; init; }
        public string? Email { get; init; }
        public bool? IsVerified { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<UserFriend> list);

    public static partial Response.UserDto? MapUserDtoInternal(User? user);

    public static Response.UserDto? ToUserDto(User user) => MapUserDtoInternal(user);
}
