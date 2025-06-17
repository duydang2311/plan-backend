using System.Text.Json;
using System.Text.Json.Nodes;
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
            && string.IsNullOrEmpty(command.SelectWorkspaceInvitation)
            && string.IsNullOrEmpty(command.SelectStatus)
                ? null
                : "Notification.Type,Notification.Data"
            : command.Select;

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
        var projectIdsToLoad = new HashSet<ProjectId>();
        var issueIdsToLoad = new HashSet<IssueId>();
        var issueAuditIdsToLoad = new HashSet<long>();
        var projectMemberInvitationIdsToLoad = new HashSet<ProjectMemberInvitationId>();
        var workspaceInvitationIdsToLoad = new HashSet<WorkspaceInvitationId>();
        var statusIdsToLoad = new HashSet<StatusId>();

        foreach (var un in userNotifications)
        {
            if (un.Notification?.Data is null)
            {
                continue;
            }

            switch (un.Notification.Type)
            {
                case NotificationType.ProjectCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectProject)
                        && un.Notification.Data.RootElement.TryGetProperty("projectId", out var projIdElem)
                        && Guid.TryParseExact(projIdElem.GetString(), "D", out var projGuid)
                    )
                    {
                        projectIdsToLoad.Add(new ProjectId { Value = projGuid });
                    }
                    break;
                case NotificationType.IssueCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectIssue)
                        && un.Notification.Data.RootElement.TryGetProperty("issueId", out var issueIdElem)
                        && Guid.TryParseExact(issueIdElem.GetString(), "D", out var issueGuid)
                    )
                    {
                        issueIdsToLoad.Add(new IssueId { Value = issueGuid });
                    }
                    break;
                case NotificationType.IssueCommentCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectComment)
                        && un.Notification.Data.RootElement.TryGetProperty("issueAuditId", out var auditIdElem)
                        && auditIdElem.TryGetInt64(out var auditId)
                    )
                    {
                        issueAuditIdsToLoad.Add(auditId);
                    }
                    break;
                case NotificationType.ProjectMemberInvited:
                    if (
                        !string.IsNullOrEmpty(command.SelectProjectMemberInvitation)
                        && un.Notification.Data.RootElement.TryGetProperty(
                            "projectMemberInvitationId",
                            out var inviteIdElem
                        )
                        && inviteIdElem.TryGetInt64(out var inviteIdValue)
                    )
                    {
                        projectMemberInvitationIdsToLoad.Add(new ProjectMemberInvitationId { Value = inviteIdValue });
                    }
                    break;
                case NotificationType.WorkspaceMemberInvited:
                {
                    if (
                        !string.IsNullOrEmpty(command.SelectWorkspaceInvitation)
                        && un.Notification.Data.RootElement.TryGetProperty("workspaceInvitationId", out var idElement)
                        && idElement.TryGetInt64(out var idValue)
                    )
                    {
                        workspaceInvitationIdsToLoad.Add(new WorkspaceInvitationId { Value = idValue });
                    }
                    break;
                }
                case NotificationType.IssueStatusUpdated:
                {
                    if (!string.IsNullOrEmpty(command.SelectStatus))
                    {
                        if (
                            un.Notification.Data.RootElement.TryGetProperty("oldStatusId", out var statusIdElement)
                            && statusIdElement.TryGetInt64(out var statusIdValue)
                        )
                        {
                            statusIdsToLoad.Add(new StatusId { Value = statusIdValue });
                        }
                        if (
                            un.Notification.Data.RootElement.TryGetProperty("newStatusId", out statusIdElement)
                            && statusIdElement.TryGetInt64(out statusIdValue)
                        )
                        {
                            statusIdsToLoad.Add(new StatusId { Value = statusIdValue });
                        }
                    }
                    break;
                }
            }
        }

        var projectsMap =
            !string.IsNullOrEmpty(command.SelectProject) && projectIdsToLoad.Count > 0
                ? await db
                    .Projects.Where(p => projectIdsToLoad.Contains(p.Id))
                    .Select(ExpressionHelper.Select<Project, Project>(command.SelectProject))
                    .ToDictionaryAsync(p => p.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var issuesMap =
            !string.IsNullOrEmpty(command.SelectIssue) && issueIdsToLoad.Count > 0
                ? await db
                    .Issues.Where(i => issueIdsToLoad.Contains(i.Id))
                    .Select(ExpressionHelper.Select<Issue, Issue>(command.SelectIssue))
                    .ToDictionaryAsync(i => i.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var auditsMap =
            !string.IsNullOrEmpty(command.SelectComment) && issueAuditIdsToLoad.Count > 0
                ? await db
                    .IssueAudits.Where(a => issueAuditIdsToLoad.Contains(a.Id))
                    .Select(ExpressionHelper.Select<IssueAudit, IssueAudit>(command.SelectComment))
                    .ToDictionaryAsync(a => a.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var projectMemberInvitationsMap =
            !string.IsNullOrEmpty(command.SelectProjectMemberInvitation) && projectMemberInvitationIdsToLoad.Count > 0
                ? await db
                    .ProjectMemberInvitations.Where(inv => projectMemberInvitationIdsToLoad.Contains(inv.Id))
                    .Select(
                        ExpressionHelper.Select<ProjectMemberInvitation, ProjectMemberInvitation>(
                            command.SelectProjectMemberInvitation
                        )
                    )
                    .ToDictionaryAsync(inv => inv.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var workspaceInvitationsMap =
            !string.IsNullOrEmpty(command.SelectWorkspaceInvitation) && workspaceInvitationIdsToLoad.Count > 0
                ? await db
                    .WorkspaceInvitations.Where(a => workspaceInvitationIdsToLoad.Contains(a.Id))
                    .Select(
                        ExpressionHelper.Select<WorkspaceInvitation, WorkspaceInvitation>(
                            command.SelectWorkspaceInvitation
                        )
                    )
                    .ToDictionaryAsync(inv => inv.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var statusesMap =
            !string.IsNullOrEmpty(command.SelectStatus) && statusIdsToLoad.Count > 0
                ? await db
                    .Statuses.Where(inv => statusIdsToLoad.Contains(inv.Id))
                    .Select(ExpressionHelper.Select<Status, Status>(command.SelectStatus))
                    .ToDictionaryAsync(inv => inv.Id, ct)
                    .ConfigureAwait(false)
                : [];

        var finalItems = new List<UserNotification>(userNotifications.Count);
        foreach (var originalUserNotification in userNotifications)
        {
            var currentItem = originalUserNotification;
            if (currentItem.Notification?.Data is null)
            {
                finalItems.Add(currentItem);
                continue;
            }

            Attempt<JsonDocument, Exception>? serializeAttempt = default;

            // TODO: Strategy pattern here when types go wild
            switch (currentItem.Notification.Type)
            {
                case NotificationType.ProjectCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectProject)
                        && currentItem.Notification.Data.RootElement.TryGetProperty("projectId", out var projIdElem)
                        && Guid.TryParseExact(projIdElem.GetString(), "D", out var projGuid)
                    )
                    {
                        var projectId = new ProjectId { Value = projGuid };
                        if (projectsMap.TryGetValue(projectId, out var project))
                        {
                            serializeAttempt = Attempt(() =>
                                JsonSerializer.SerializeToDocument(project, jsonOptions.Value.SerializerOptions)
                            );
                        }
                    }
                    break;
                case NotificationType.IssueCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectIssue)
                        && currentItem.Notification.Data.RootElement.TryGetProperty("issueId", out var issueIdElem)
                        && Guid.TryParseExact(issueIdElem.GetString(), "D", out var issueGuid)
                    )
                    {
                        var issueId = new IssueId { Value = issueGuid };
                        if (issuesMap.TryGetValue(issueId, out var issue))
                        {
                            serializeAttempt = Attempt(() =>
                                JsonSerializer.SerializeToDocument(issue, jsonOptions.Value.SerializerOptions)
                            );
                        }
                    }
                    break;
                case NotificationType.IssueCommentCreated:
                    if (
                        !string.IsNullOrEmpty(command.SelectComment)
                        && currentItem.Notification.Data.RootElement.TryGetProperty("issueAuditId", out var auditIdElem)
                        && auditIdElem.TryGetInt64(out var auditId)
                    )
                    {
                        if (auditsMap.TryGetValue(auditId, out var audit))
                        {
                            serializeAttempt = Attempt(() =>
                                JsonSerializer.SerializeToDocument(audit, jsonOptions.Value.SerializerOptions)
                            );
                        }
                    }
                    break;
                case NotificationType.ProjectMemberInvited:
                    if (
                        !string.IsNullOrEmpty(command.SelectProjectMemberInvitation)
                        && currentItem.Notification.Data.RootElement.TryGetProperty(
                            "projectMemberInvitationId",
                            out var inviteIdElem
                        )
                        && inviteIdElem.TryGetInt64(out var inviteIdValue)
                    )
                    {
                        var inviteId = new ProjectMemberInvitationId { Value = inviteIdValue };
                        if (projectMemberInvitationsMap.TryGetValue(inviteId, out var invitation))
                        {
                            serializeAttempt = Attempt(() =>
                                JsonSerializer.SerializeToDocument(invitation, jsonOptions.Value.SerializerOptions)
                            );
                        }
                    }
                    break;
                case NotificationType.WorkspaceMemberInvited:
                {
                    if (
                        !string.IsNullOrEmpty(command.SelectWorkspaceInvitation)
                        && currentItem.Notification.Data.RootElement.TryGetProperty(
                            "workspaceInvitationId",
                            out var idElement
                        )
                        && idElement.TryGetInt64(out var idValue)
                    )
                    {
                        var id = new WorkspaceInvitationId { Value = idValue };
                        if (workspaceInvitationsMap.TryGetValue(id, out var invitation))
                        {
                            serializeAttempt = Attempt(() =>
                                JsonSerializer.SerializeToDocument(invitation, jsonOptions.Value.SerializerOptions)
                            );
                        }
                    }
                    break;
                }
                case NotificationType.IssueStatusUpdated:
                {
                    if (string.IsNullOrEmpty(command.SelectStatus))
                    {
                        break;
                    }
                    var jsonObject = new JsonObject();
                    if (
                        currentItem.Notification.Data.RootElement.TryGetProperty("oldStatusId", out var idElement)
                        && idElement.TryGetInt64(out var idValue)
                    )
                    {
                        var id = new StatusId { Value = idValue };
                        if (statusesMap.TryGetValue(id, out var status))
                        {
                            jsonObject["oldStatus"] = JsonSerializer.SerializeToNode(
                                status,
                                jsonOptions.Value.SerializerOptions
                            );
                        }
                    }
                    if (
                        currentItem.Notification.Data.RootElement.TryGetProperty("newStatusId", out idElement)
                        && idElement.TryGetInt64(out idValue)
                    )
                    {
                        var id = new StatusId { Value = idValue };
                        if (statusesMap.TryGetValue(id, out var status))
                        {
                            jsonObject["newStatus"] = JsonSerializer.SerializeToNode(
                                status,
                                jsonOptions.Value.SerializerOptions
                            );
                        }
                    }
                    if (jsonObject.Count > 0)
                    {
                        serializeAttempt = Attempt(() =>
                            JsonSerializer.SerializeToDocument(jsonObject, jsonOptions.Value.SerializerOptions)
                        );
                    }
                    break;
                }
            }

            if (serializeAttempt is not null && serializeAttempt.TryGetData(out var data, out _))
            {
                currentItem = currentItem with { Notification = currentItem.Notification with { Data = data } };
            }

            finalItems.Add(currentItem);
        }

        return PaginatedList.From(finalItems, totalCount);
    }
}
