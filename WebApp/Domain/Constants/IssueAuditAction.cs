namespace WebApp.Domain.Constants;

public enum IssueAuditAction : byte
{
    None,
    Create,
    UpdateTitle,
    UpdateDescription,
    Comment,
}
