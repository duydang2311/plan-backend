namespace WebApp.Domain.Entities;

public sealed record RolePermission
{
    public RoleId RoleId { get; init; }
    public string Permission { get; init; } = string.Empty;
}
