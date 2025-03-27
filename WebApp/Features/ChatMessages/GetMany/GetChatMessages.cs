using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChatMessages.GetMany;

public sealed record GetChatMessages : KeysetPagination<ChatMessageId?>, ICommand<PaginatedList<ChatMessage>>
{
    public required ChatId ChatId { get; init; }
    public string? Select { get; init; }
}
