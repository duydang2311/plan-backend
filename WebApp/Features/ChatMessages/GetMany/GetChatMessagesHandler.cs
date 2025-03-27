using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChatMessages.GetMany;

public sealed record GetChatMessageHandler(AppDbContext db)
    : ICommandHandler<GetChatMessages, PaginatedList<ChatMessage>>
{
    public async Task<PaginatedList<ChatMessage>> ExecuteAsync(GetChatMessages command, CancellationToken ct)
    {
        var query = db.ChatMessages.Where(a => a.ChatId == command.ChatId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (command.Cursor.HasValue)
        {
            query = query.Where(a => a.Id < command.Cursor);
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ChatMessage, ChatMessage>(command.Select));
        }

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        return PaginatedList.From(await query.Take(command.Size).ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
