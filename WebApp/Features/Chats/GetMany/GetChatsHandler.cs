using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using WebApp.Common.Constants;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Chats.GetMany;

public sealed class GetChatsHandler(AppDbContext db) : ICommandHandler<GetChats, PaginatedList<Chat>>
{
    public async Task<PaginatedList<Chat>> ExecuteAsync(GetChats command, CancellationToken ct)
    {
        var query = db.Chats.AsSplitQuery();

        if (command.UserId.HasValue)
        {
            query = query.Where(a => a.ChatMembers.Any(b => b.MemberId == command.UserId));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            // Console.WriteLine("Expression is: " + ExpressionHelper.Select<Chat, Chat>(command.Select).Body.ToString());
            var parameter = Expression.Parameter(typeof(Chat), "a");
            var selectExpression = ExpressionHelper.Select<Chat, Chat>(command.Select, parameter);

            if (!string.IsNullOrEmpty(command.SelectLastChatMessage))
            {
                var selectLastChatMessageExpression = ExpressionHelper.Select<ChatMessage, ChatMessage>(
                    command.SelectLastChatMessage
                );
                // selectExpression = selectExpression.Update(
                //     ((MemberInitExpression)selectExpression.Body).Update(((MemberInitExpression)selectExpression.Body).NewExpression, (MemberInitExpression)selectExpression.Body).Bindings.Append(

                //     ))
                // );
                var fixedMemberInitExpression = Expression.MemberInit(
                    Expression.New(typeof(Chat).GetConstructor(Type.EmptyTypes)!),
                    ((MemberInitExpression)selectExpression.Body).Bindings.Select(a =>
                        a.Member.Name.Equals("LastChatMessage")
                            ? Expression.Bind(
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
                                            selectLastChatMessageExpression
                                        ),
                                        Expression.Lambda<Func<ChatMessage, Instant>>(
                                            Expression.Property(parameter, nameof(ChatMessage.CreatedTime)),
                                            Expression.Parameter(typeof(ChatMessage), "b")
                                        )
                                    )
                                )
                            )
                            : a
                    )
                );
                selectExpression = Expression.Lambda<Func<Chat, Chat>>(fixedMemberInitExpression, parameter);
            }
            query = query.Select(selectExpression);
        }

        foreach (var orderable in command.Order)
        {
            if (orderable.Name.Equals("LastChatMessage.CreatedTime"))
            {
                query = orderable.Order switch
                {
                    Common.Constants.Order.Descending => query.OrderByDescending(a => a.LastChatMessage!.CreatedTime),
                    _ => query.OrderBy(a => a.LastChatMessage!.CreatedTime),
                };
            }
            else
            {
                query = orderable.Sort(query);
            }
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
