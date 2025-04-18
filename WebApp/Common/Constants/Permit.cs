namespace WebApp.Common.Constants;

public static class Permit
{
    public const string Read = "read";

    public const string ReadTeam = "team:read";
    public const string CreateTeam = "team:create";

    public const string CreateIssue = "issue:create";
    public const string ReadIssue = "issue:read";
    public const string UpdateIssue = "issue:update";
    public const string DeleteIssue = "issue:delete";
    public const string CommentIssue = "issue:comment";

    public const string ReadIssueComment = "issue-comment:read";
    public const string DeleteIssueComment = "issue-comment:delete";

    public const string UpdateTeamRole = "team-role:update";

    public const string CreateTeamMember = "team-member:create";
    public const string ReadTeamMember = "team-member:read";

    public const string ReadTeamInvitation = "team-invitation:read";

    public const string CreateProject = "project:create";
    public const string ReadProject = "project:read";
    public const string DeleteProject = "project:delete";

    public const string ReadWorkspaceStatus = "workspace-status:read";
    public const string CreateWorkspaceStatus = "workspace-status:create";
    public const string DeleteWorkspaceStatus = "workspace-status:delete";
    public const string UpdateWorkspaceStatus = "workspace-status:update";

    public const string ReadWorkspaceMember = "workspace-member:read";
    public const string DeleteWorkspaceMember = "workspace-member:delete";

    public const string ReadIssueAudit = "issue-audit:read";
    public const string DeleteIssueAudit = "issue-audit:delete";
    public const string CreateIssueAuditComment = "issue-audit-comment:create";

    public const string ReadProjectMember = "project-member:read";
    public const string DeleteProjectMember = "project-member:delete";
    public const string CreateProjectMember = "project-member:create";
    public const string UpdateProjectMember = "project-member:update";

    public const string ReadProjectMemberInvitation = "project-member-invitation:read";
    public const string DeleteProjectMemberInvitation = "project-member-invitation:delete";
    public const string CreateProjectMemberInvitation = "project-member-invitation:create";
}
