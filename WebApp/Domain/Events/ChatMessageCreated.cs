using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ChatMessageCreated : ICommand
{
    public required ChatId ChatId { get; init; }
    public required ChatMessageId ChatMessageId { get; init; }
    public string? OptimisticId { get; init; }
}
