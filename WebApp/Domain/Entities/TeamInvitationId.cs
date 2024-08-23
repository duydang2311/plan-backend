namespace WebApp.Domain.Entities;

public readonly record struct TeamInvitationId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly TeamInvitationId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
