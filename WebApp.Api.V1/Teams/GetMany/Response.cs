using NodaTime;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetMany;

public sealed record Response : PaginatedList<Item> { }

public sealed record Item
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public TeamId Id { get; init; } = TeamId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public ICollection<User> Members { get; set; } = null!;

    public static Item From(Team team) =>
        new()
        {
            CreatedTime = team.CreatedTime,
            UpdatedTime = team.UpdatedTime,
            Id = team.Id,
            WorkspaceId = team.WorkspaceId,
            Name = team.Name,
            Identifier = team.Identifier,
            Members = team.Members,
        };
}
