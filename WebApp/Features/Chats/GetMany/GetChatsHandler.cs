using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Chats.GetMany;

public sealed class GetChatsHandler(AppDbContext db) : ICommandHandler<GetChats, PaginatedList<Chat>>
{
    public async Task<PaginatedList<Chat>> ExecuteAsync(GetChats command, CancellationToken ct)
    {
        var query = db.Chats.AsQueryable();

        if (command.UserId.HasValue)
        {
            query = query.Where(a => a.ChatMembers.Any(b => b.MemberId == command.UserId));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var chatMessageCreatedTimeOrder = command.Order.FirstOrDefault(a =>
            a.Name.Equals("ChatMessageCreatedTime", StringComparison.OrdinalIgnoreCase)
        );
        if (chatMessageCreatedTimeOrder is not null)
        {
            query = chatMessageCreatedTimeOrder.Order switch
            {
                Common.Constants.Order.Ascending => query.OrderBy(a => a.ChatMessages.Min(b => b.CreatedTime)),
                Common.Constants.Order.Descending => query.OrderByDescending(a =>
                    a.ChatMessages.Max(b => b.CreatedTime)
                ),
                _ => query,
            };
        }
        query = command
            .Order.Where(a => !a.Name.EqualsEither(["ChatMessageCreatedTime"]))
            .SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        if (!string.IsNullOrEmpty(command.Select))
        {
            var chatParameter = Expression.Parameter(typeof(Chat), "a");
            var selectExpression = ExpressionHelper.Select<Chat, Chat>(command.Select, chatParameter);
            var bindings = new List<MemberBinding>(2);

            if (command.FilterChatMemberId.HasValue || !string.IsNullOrEmpty(command.SelectChatMember))
            {
                bindings.Add(ChatMemberBinding(chatParameter, command.FilterChatMemberId, command.SelectChatMember));
            }

            if (command.SelectLastChatMessage is not null)
            {
                bindings.Add(ChatMessageBinding(chatParameter, command.SelectLastChatMessage));
            }

            if (bindings.Count > 0)
            {
                selectExpression = Expression.Lambda<Func<Chat, Chat>>(
                    Expression.MemberInit(
                        Expression.New(typeof(Chat).GetConstructor(Type.EmptyTypes)!),
                        [.. ((MemberInitExpression)selectExpression.Body).Bindings, .. bindings]
                    ),
                    chatParameter
                );
            }

            query = query.Select(selectExpression);
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).AsSplitQuery().ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }

    static MemberAssignment ChatMessageBinding(ParameterExpression parameter, string selectChatMessage)
    {
        return Expression.Bind(
            typeof(Chat).GetProperty(nameof(Chat.LastChatMessage))!,
            Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.FirstOrDefault),
                [typeof(ChatMessage)],
                Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.OrderByDescending),
                    [typeof(ChatMessage), typeof(Instant)],
                    Expression.Call(
                        typeof(Enumerable),
                        nameof(Enumerable.Select),
                        [typeof(ChatMessage), typeof(ChatMessage)],
                        Expression.Property(parameter, nameof(Chat.ChatMessages)),
                        ExpressionHelper.Select<ChatMessage, ChatMessage>(selectChatMessage)
                    ),
                    Expression.Lambda<Func<ChatMessage, Instant>>(
                        Expression.Property(parameter, nameof(ChatMessage.CreatedTime)),
                        Expression.Parameter(typeof(ChatMessage), "b")
                    )
                )
            )
        );
    }

    static MemberAssignment ChatMemberBinding(ParameterExpression chatParameter, UserId? memberId, string? select)
    {
        var chatMemberParameter = Expression.Parameter(typeof(ChatMember), "b");
        MethodCallExpression? expression = default;

        if (memberId.HasValue)
        {
            expression = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Where),
                [typeof(ChatMember)],
                Expression.Property(chatParameter, nameof(Chat.ChatMembers)),
                Expression.Lambda<Func<ChatMember, bool>>(
                    Expression.Equal(
                        Expression.Property(chatMemberParameter, nameof(ChatMember.MemberId)),
                        Expression.Constant(memberId, typeof(UserId))
                    ),
                    chatMemberParameter
                )
            );
        }

        if (!string.IsNullOrEmpty(select))
        {
            expression = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Select),
                [typeof(ChatMember), typeof(ChatMember)],
                (Expression?)expression ?? Expression.Property(chatParameter, nameof(Chat.ChatMembers)),
                ExpressionHelper.Select<ChatMember, ChatMember>(select)
            );
        }

        expression = Expression.Call(
            typeof(Enumerable),
            nameof(Enumerable.ToList),
            [typeof(ChatMember)],
            (Expression?)expression ?? Expression.Property(chatParameter, nameof(Chat.ChatMembers))
        );

        return Expression.Bind(typeof(Chat).GetProperty(nameof(Chat.ChatMembers))!, expression);
    }
}
