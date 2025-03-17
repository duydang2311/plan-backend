using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.UserFriendRequests.GetMany;

namespace WebApp.Api.V1.UserFriendRequests.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant? CreatedTime { get; init; }
        public UserId? SenderId { get; init; }
        public UserDto? Sender { get; init; }
        public UserId? ReceiverId { get; init; }
        public UserDto? Receiver { get; init; }
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
    public static partial Response ToResponse(this PaginatedList<UserFriendRequest> list);

    public static partial Response.UserDto? MapUserDtoInternal(User? user);

    public static Response.UserDto? ToUserDto(User user) => MapUserDtoInternal(user);
}
