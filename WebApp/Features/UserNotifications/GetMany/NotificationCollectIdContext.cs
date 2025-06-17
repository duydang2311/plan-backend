using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class NotificationCollectIdContext
{
    public HashSet<ProjectId> ProjectIds { get; } = [];
    public HashSet<IssueId> IssueIds { get; } = [];
    public HashSet<long> IssueAuditIds { get; } = [];
    public HashSet<ProjectMemberInvitationId> ProjectMemberInvitationIds { get; } = [];
    public HashSet<WorkspaceInvitationId> WorkspaceInvitationIds { get; } = [];
    public HashSet<StatusId> StatusIds { get; } = [];
}
