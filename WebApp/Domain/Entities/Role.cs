namespace WebApp.Domain.Entities;

public sealed record Role
{
    public RoleId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Rank { get; init; }
    public ICollection<RolePermission> Permissions { get; init; } = null!;
}
