namespace WebApp.Domain.Entities;

public sealed record TeamRolePermission
{
    public TeamRoleId RoleId { get; init; }
    public string Permission { get; init; } = string.Empty;
}
