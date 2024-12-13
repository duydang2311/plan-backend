namespace WebApp.Domain.Entities;

public readonly record struct WorkspaceInvitationId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly WorkspaceInvitationId Empty;
}
