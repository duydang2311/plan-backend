using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class NotificationHydrateContext
{
    public IReadOnlyDictionary<ProjectId, Project> ProjectsMap { get; set; } = new Dictionary<ProjectId, Project>();
    public IReadOnlyDictionary<IssueId, Issue> IssuesMap { get; set; } = new Dictionary<IssueId, Issue>();
    public IReadOnlyDictionary<long, IssueAudit> AuditsMap { get; set; } = new Dictionary<long, IssueAudit>();
    public IReadOnlyDictionary<
        ProjectMemberInvitationId,
        ProjectMemberInvitation
    > ProjectMemberInvitationsMap { get; set; } = new Dictionary<ProjectMemberInvitationId, ProjectMemberInvitation>();
    public IReadOnlyDictionary<WorkspaceInvitationId, WorkspaceInvitation> WorkspaceInvitationsMap { get; set; } =
        new Dictionary<WorkspaceInvitationId, WorkspaceInvitation>();
    public IReadOnlyDictionary<StatusId, WorkspaceStatus> StatusesMap { get; set; } =
        new Dictionary<StatusId, WorkspaceStatus>();
}
