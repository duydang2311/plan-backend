using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChatMessages.GetOne;

public sealed record GetChatMessageHandler(AppDbContext db)
    : ICommandHandler<GetChatMessage, OneOf<NotFoundError, ChatMessage>>
{
    public async Task<OneOf<NotFoundError, ChatMessage>> ExecuteAsync(GetChatMessage command, CancellationToken ct)
    {
        var query = db.ChatMessages.Where(a => a.Id == command.ChatMessageId);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ChatMessage, ChatMessage>(command.Select));
        }

        var chatMessage = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (chatMessage is null)
        {
            return new NotFoundError();
        }

        return chatMessage;
    }
}
