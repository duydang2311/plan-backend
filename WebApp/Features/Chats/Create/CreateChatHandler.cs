using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Chats.Create;

public sealed record CreateChatHandler(AppDbContext db)
    : ICommandHandler<CreateChat, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(CreateChat command, CancellationToken ct)
    {
        if (command.MemberIds.Count == 0)
        {
            return ValidationFailures.Single("memberIds", "Chat must have at least one member", "invalid_member_count");
        }

        var chat = new Chat
        {
            OwnerId = command.OwnerId,
            ChatMembers = [.. command.MemberIds.Select(a => new ChatMember { MemberId = a })],
            Type = command.MemberIds.Count switch
            {
                1 => ChatType.OneOnOne,
                0 => throw new InvalidProgramException(),
                _ => ChatType.Group,
            },
            Title = command.MemberIds.Count switch
            {
                1 => null,
                0 => throw new InvalidProgramException(),
                _ => "Group chat",
            },
        };

        db.Add(chat);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            return ValidationFailures.Single("memberIds", "Chat with these members already exists", "chat_exists");
        }
        return new Success();
    }
}
