using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseChatMessageDto
{
    public Instant? CreatedTime { get; init; }
    public ChatMessageId? Id { get; init; }
    public ChatId? ChatId { get; init; }
    public UserId? SenderId { get; init; }
    public BaseUserDto? Sender { get; init; }
    public string? Content { get; init; }
}
