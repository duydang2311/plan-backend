using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChatMessages.Create;

public sealed record CreateChatMessage : ICommand<OneOf<ValidationFailures, ChatMessage>>
{
    public required ChatId ChatId { get; init; }
    public required UserId SenderId { get; init; }
    public required string Content { get; init; }
}
