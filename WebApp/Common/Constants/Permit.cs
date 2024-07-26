namespace WebApp.Common.Constants;

public static class Permit
{
    public const string Read = "read";

    public const string ReadTeam = "team:read";
    public const string WriteTeam = "team:write";

    public const string ReadIssue = "issue:read";
    public const string CreateIssue = "issue:create";
    public const string CommentIssue = "issue:comment";
}
