using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChatMessages.GetOne;

public sealed record GetChatMessage : ICommand<OneOf<NotFoundError, ChatMessage>>
{
    public required ChatMessageId ChatMessageId { get; init; }
    public string? Select { get; init; }
}
