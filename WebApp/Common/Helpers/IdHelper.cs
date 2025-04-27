using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.Domain.Entities;

namespace WebApp.Common.Helpers;

public static class IdHelper
{
    public static Guid NewGuid()
    {
        return Guid.NewGuid();
    }

    public static string NewRandomId()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Base64UrlTextEncoder.Encode(bytes);
    }

    public static UserId NewUserId() => new() { Value = NewGuid() };

    public static WorkspaceId NewWorkspaceId() => new() { Value = NewGuid() };

    public static TeamId NewTeamId() => new() { Value = NewGuid() };

    public static IssueId NewIssueId() => new() { Value = NewGuid() };

    public static SessionToken NewSessionId() => new() { Value = NewGuid() };

    public static ProjectId NewProjectId() => new() { Value = NewGuid() };
}
