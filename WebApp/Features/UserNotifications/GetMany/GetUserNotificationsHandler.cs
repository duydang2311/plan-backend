using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class GetUserNotificationsHandler(AppDbContext db, IOptions<JsonOptions> jsonOptions)
    : ICommandHandler<GetUserNotifications, PaginatedList<UserNotification>>
{
    public async Task<PaginatedList<UserNotification>> ExecuteAsync(GetUserNotifications command, CancellationToken ct)
    {
        var query = db.UserNotifications.Where(a => a.UserId == command.UserId);
        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var select = string.IsNullOrEmpty(command.Select)
            ? string.IsNullOrEmpty(command.SelectProject)
            && string.IsNullOrEmpty(command.SelectIssue)
            && string.IsNullOrEmpty(command.SelectComment)
            && string.IsNullOrEmpty(command.SelectWorkspaceInvitation)
            && string.IsNullOrEmpty(command.SelectStatus)
                ? null
                : "Notification.Type,Notification.Data"
            : command.Select;

        Console.WriteLine("command.Order = " + string.Join(',', command.Order.Select(a => $"{a.Name} {a.Order}")));
        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        if (!string.IsNullOrEmpty(select))
        {
            query = query.Select(ExpressionHelper.Select<UserNotification, UserNotification>(select));
        }
        if (command.Cursor is not null)
        {
            query = query.Where(a => a.Id < command.Cursor);
        }

        if (command.Cursor is null)
        {
            query = query.Skip(command.Offset);
        }

        var userNotifications = await query.Take(command.Size).ToListAsync(ct).ConfigureAwait(false);

        var strategyFactory = new NotificationStrategyFactory(
            new ProjectCreatedNotificationStrategy(),
            new IssueCreatedNotificationStrategy(),
            new IssueCommentCreatedNotificationStrategy(),
            new ProjectMemberInvitedNotificationStrategy(),
            new WorkspaceMemberInvitedNotificationStrategy(),
            new IssueStatusUpdatedNotificationStrategy()
        );
        var loadContext = new NotificationLoadContext(
            new NotificationCollectIdContext(),
            new NotificationHydrateContext(),
            command,
            db
        );

        foreach (var un in userNotifications)
        {
            strategyFactory.GetStrategy(un.Notification.Type).CollectIds(un, command, loadContext.CollectIdContext);
        }

        foreach (var type in userNotifications.Select(a => a.Notification.Type).Distinct())
        {
            await strategyFactory.GetStrategy(type).LoadDataAsync(loadContext, ct).ConfigureAwait(false);
        }

        var finalItems = new List<UserNotification>(userNotifications.Count);
        foreach (var un in userNotifications)
        {
            finalItems.Add(
                strategyFactory
                    .GetStrategy(un.Notification.Type)
                    .HydrateData(un, command, loadContext.HydrateContext, jsonOptions.Value.SerializerOptions)
            );
        }
        return PaginatedList.From(finalItems, totalCount);
    }
}
