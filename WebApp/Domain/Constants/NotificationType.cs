namespace WebApp.Domain.Constants;

public enum NotificationType : byte
{
    None,
    ProjectCreated,
    IssueCreated,
    IssueCommentCreated,
    ProjectMemberInvited,
    WorkspaceMemberInvited,
    IssueStatusUpdated,
}
