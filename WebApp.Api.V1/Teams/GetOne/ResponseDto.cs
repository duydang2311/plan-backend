using NodaTime;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Teams.GetOne;

public sealed record ResponseDto
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public TeamId Id { get; init; } = TeamId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public ICollection<User> Members { get; set; } = null!;

    public static ResponseDto From(Team team) =>
        new()
        {
            CreatedTime = team.CreatedTime,
            UpdatedTime = team.UpdatedTime,
            Id = team.Id,
            WorkspaceId = team.WorkspaceId,
            Name = team.Name,
            Identifier = team.Identifier,
            Members = team.Members
        };
}
