using System.Security.Cryptography;
using WebApp.Domain.Entities;

namespace WebApp.SharedKernel.Helpers;

public static class IdHelper
{
    public static Guid NewGuid()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        RandomNumberGenerator.Fill(guidBytes);
        return new Guid(guidBytes);
    }

    public static UserId NewUserId() => new(NewGuid());

    public static WorkspaceId NewWorkspaceId() => new(NewGuid());

    public static TeamId NewTeamId() => new(NewGuid());
}
