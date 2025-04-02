using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseChatMemberDto
{
    public Instant? CreatedTime { get; init; }
    public ChatId? ChatId { get; init; }
    public BaseChatDto? Chat { get; init; } = null!;
    public UserId? MemberId { get; init; }
    public BaseUserDto? Member { get; init; } = null!;
    public ChatMessageId? LastReadMessageId { get; init; }
    public BaseChatMessageDto? LastReadMessage { get; init; }
}
