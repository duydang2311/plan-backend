using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChatMessages.Create;

public sealed record CreateChatMessageHandler(AppDbContext db, ILogger<CreateChatMessageHandler> logger)
    : ICommandHandler<CreateChatMessage, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(CreateChatMessage command, CancellationToken ct)
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

        var createdEvent = new ChatMessageCreated { ChatMessageId = chatMessage.Id, ChatId = chatMessage.ChatId };
        db.Add(createdEvent.CreateJob<JobRecord>(executeAfter: DateTime.UtcNow));
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            await transaction.CommitAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to save job record for chat message created event");
            throw;
        }

        createdEvent.TriggerJobExecution();

        return new Success();
    }
}
