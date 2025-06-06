using EntityFrameworkCore.Projectables;
using NodaTime;
using WebApp.Common.Interfaces;

namespace WebApp.Domain.Entities;

public sealed record class Workspace : ISoftDelete
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
    public Instant? DeletedTime { get; init; }

    [Projectable(UseMemberBody = nameof(totalProjects))]
    public int TotalProjects { get; init; }

    [Projectable(UseMemberBody = nameof(totalUsers))]
    public int TotalUsers { get; init; }

    private int totalProjects => Projects.Count;
    private int totalUsers => Users.Count;

    public ICollection<User> Users { get; init; } = null!;

    public ICollection<WorkspaceStatus> Statuses
    {
        get => statuses ??= [];
        set { statuses = value; }
    }

    public ICollection<WorkspaceFieldDefinition> FieldDefinitions
    {
        get => fieldDefinitions ??= [];
        set { fieldDefinitions = value; }
    }

    public ICollection<WorkspaceMember> Members
    {
        get => members ??= [];
        set { members = value; }
    }

    public ICollection<Project> Projects
    {
        get => projects ??= [];
        set { projects = value; }
    }
}
