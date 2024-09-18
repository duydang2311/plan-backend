using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class Workspace
{
    private ICollection<WorkspaceStatus>? statuses;
    private ICollection<WorkspaceFieldDefinition>? fieldDefinitions;
    private ICollection<WorkspaceMember>? members;
    private ICollection<Project>? projects;

    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId Id { get; init; } = WorkspaceId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;

    public ICollection<WorkspaceStatus> Statuses => statuses ??= [];
    public ICollection<WorkspaceFieldDefinition> FieldDefinitions => fieldDefinitions ??= [];
    public ICollection<WorkspaceMember> Members => members ??= [];
    public ICollection<Project> Projects => projects ??= [];
}
