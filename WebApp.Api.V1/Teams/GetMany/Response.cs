using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetMany;

public sealed record Response : PaginatedList<Item> { }

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Team> paginatedList);
}

public sealed record Item
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public TeamId Id { get; init; } = TeamId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public ICollection<User> Members { get; set; } = null!;
}
