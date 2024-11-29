using System.Security.Cryptography;
using WebApp.Domain.Entities;

namespace WebApp.Common.Helpers;

public static class IdHelper
{
    public static Guid NewGuid()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        RandomNumberGenerator.Fill(guidBytes);
        return new Guid(guidBytes);
    }

    public static UserId NewUserId() => new() { Value = NewGuid() };

    public static WorkspaceId NewWorkspaceId() => new() { Value = NewGuid() };

    public static TeamId NewTeamId() => new() { Value = NewGuid() };

    public static IssueId NewIssueId() => new() { Value = NewGuid() };

    public static SessionToken NewSessionId() => new() { Value = NewGuid() };

    public static ProjectId NewProjectId() => new() { Value = NewGuid() };
}
