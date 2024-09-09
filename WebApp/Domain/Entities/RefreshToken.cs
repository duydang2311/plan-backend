namespace WebApp.Domain.Entities;

public readonly record struct RefreshToken(Guid Value) : IEntityGuid
{
    public static readonly RefreshToken Empty = new(Guid.Empty);
}
