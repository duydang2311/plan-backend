namespace WebApp.Domain.Entities;

public readonly record struct WorkspaceMemberId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly WorkspaceMemberId Empty;
}
