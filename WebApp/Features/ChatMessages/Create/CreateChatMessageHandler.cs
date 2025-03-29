using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChatMessages.Create;

public sealed record CreateChatMessageHandler(AppDbContext db)
    : ICommandHandler<CreateChatMessage, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(CreateChatMessage command, CancellationToken ct)
    {
        db.Add(
            new ChatMessage
            {
                ChatId = command.ChatId,
                SenderId = command.SenderId,
                Content = command.Content,
            }
        );

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(command.ChatId) => ("chatId", "Chat not found"),
                    nameof(command.SenderId) => ("senderId", "Sender not found"),
                    _ => null,
                }
            );
        }

        return new Success();
    }
}
