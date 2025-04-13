namespace WebApp.Infrastructure.Messaging.Common;

public sealed record ChatMessageCreatedEvent(string ChatId, long ChatMessageId, string? OptimisticId) { }
