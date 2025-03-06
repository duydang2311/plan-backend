using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Roles.GetMany;

public sealed record Response : PaginatedList<Response.Role>
{
    public sealed record Role
    {
        public RoleId? Id { get; init; }
        public string? Name { get; init; }
        public ICollection<RolePermission>? Permissions { get; init; }
    }

    public sealed record RolePermission
    {
        public RoleId? RoleId { get; init; }
        public string? Permission { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Role> list);

    public static ICollection<Response.RolePermission>? ToRolePermissions(
        this ICollection<RolePermission> collection
    ) => collection?.ToRolePermissionsInternal();

    public static partial ICollection<Response.RolePermission>? ToRolePermissionsInternal(
        this ICollection<RolePermission>? collection
    );
}
