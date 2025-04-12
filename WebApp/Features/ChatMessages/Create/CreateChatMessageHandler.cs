using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.ChatMessages.Create;

public sealed record CreateChatMessageHandler(
    AppDbContext db,
    ILogger<CreateChatMessageHandler> logger,
    IDbContextOutbox outbox
) : ICommandHandler<CreateChatMessage, OneOf<ValidationFailures, ServerError, ChatMessage>>
{
    public async Task<OneOf<ValidationFailures, ServerError, ChatMessage>> ExecuteAsync(
        CreateChatMessage command,
        CancellationToken ct
    )
    {
        var chatMessage = new ChatMessage
        {
            ChatId = command.ChatId,
            SenderId = command.SenderId,
            Content = command.Content,
        };

        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
        try
        {
            db.Add(chatMessage);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            await transaction.RollbackAsync(ct).ConfigureAwait(false);
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(command.ChatId) => ("chatId", "Chat not found"),
                    nameof(command.SenderId) => ("senderId", "Sender not found"),
                    _ => null,
                }
            );
        }

        try
        {
            outbox.Enroll(db);
            await outbox
                .PublishAsync(
                    new ChatMessageCreated
                    {
                        ChatMessageId = chatMessage.Id,
                        ChatId = chatMessage.ChatId,
                        OptimisticId = command.OptimisticId,
                    }
                )
                .ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish ChatMessageCreated message");
            return Errors.Outbox();
        }

        return chatMessage;
    }
}
