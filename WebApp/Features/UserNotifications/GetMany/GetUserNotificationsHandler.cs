using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
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
                ? null
                : "Notification.Type"
            : command.Select;
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<UserNotification, UserNotification>(command.Select));
        }

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        for (var i = 0; i != items.Count; ++i)
        {
            var item = items[i];
            if (item.Notification is null || item.Notification.Data is null)
            {
                continue;
            }

            JsonDocument? newData = default;
            switch (item.Notification.Type)
            {
                case NotificationType.ProjectCreated:
                    if (
                        string.IsNullOrEmpty(command.SelectProject)
                        || !item.Notification.Data.RootElement.TryGetProperty("projectId", out var projectIdElement)
                        || !Guid.TryParseExact(projectIdElement.GetString(), "D", out var projectGuid)
                    )
                    {
                        break;
                    }
                    var projectId = new ProjectId { Value = projectGuid };
                    newData = JsonSerializer.SerializeToDocument(
                        await db
                            .Projects.Where(a => a.Id == projectId)
                            .Select(ExpressionHelper.Select<Project, Project>(command.SelectProject))
                            .FirstOrDefaultAsync(ct)
                            .ConfigureAwait(false),
                        jsonOptions.Value.SerializerOptions
                    );
                    break;
                case NotificationType.IssueCreated:
                    if (
                        string.IsNullOrEmpty(command.SelectIssue)
                        || !item.Notification.Data.RootElement.TryGetProperty("issueId", out var issueIdElement)
                        || !Guid.TryParseExact(issueIdElement.GetString(), "D", out var issueGuid)
                    )
                    {
                        break;
                    }
                    var issueId = new IssueId { Value = issueGuid };
                    newData = JsonSerializer.SerializeToDocument(
                        await db
                            .Issues.Where(a => a.Id == issueId)
                            .Select(ExpressionHelper.Select<Issue, Issue>(command.SelectIssue))
                            .FirstOrDefaultAsync(ct)
                            .ConfigureAwait(false),
                        jsonOptions.Value.SerializerOptions
                    );
                    break;
                case NotificationType.IssueCommentCreated:
                    if (
                        string.IsNullOrEmpty(command.SelectComment)
                        || !item.Notification.Data.RootElement.TryGetProperty("issueAuditId", out var auditIdElement)
                        || !auditIdElement.TryGetInt64(out var auditId)
                    )
                    {
                        break;
                    }
                    newData = JsonSerializer.SerializeToDocument(
                        await db
                            .IssueAudits.Where(a => a.Id == auditId)
                            .Select(ExpressionHelper.Select<IssueAudit, IssueAudit>(command.SelectComment))
                            .FirstOrDefaultAsync(ct)
                            .ConfigureAwait(false),
                        jsonOptions.Value.SerializerOptions
                    );
                    break;
            }
            if (newData is not null)
            {
                items[i] = item with { Notification = item.Notification with { Data = newData } };
            }
        }
        return PaginatedList.From(items, totalCount);
    }
}
