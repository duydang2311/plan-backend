using NodaTime;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseChatDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public ChatId? Id { get; init; }
    public ChatType? Type { get; init; }
    public string? Title { get; init; }
    public Instant? DeletedTime { get; init; }
    public UserId? OwnerId { get; init; }
    public BaseUserDto? Owner { get; init; }
    public BaseChatMessageDto? LastChatMessage { get; init; }

    public ICollection<BaseUserDto>? Members { get; init; }
}
