namespace WebApp.Domain.Entities;

public sealed record TeamRole
{
    public TeamRoleId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ICollection<TeamRolePermission> Permissions { get; init; } = null!;
}
