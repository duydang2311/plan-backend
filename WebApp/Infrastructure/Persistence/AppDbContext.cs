using Casbin.Persist.Adapter.EFCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions options) : CasbinDbContext<int>(options, tableName: "policies")
{
    public DbSet<User> Users => Set<User>();
    public DbSet<JobRecord> JobRecords => Set<JobRecord>();
    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<IssueComment> IssueComments => Set<IssueComment>();
    public DbSet<TeamRole> TeamRoles => Set<TeamRole>();
    public DbSet<TeamInvitation> TeamInvitations => Set<TeamInvitation>();
    public DbSet<WorkspaceFieldDefinition> WorkspaceFieldDefinitions => Set<WorkspaceFieldDefinition>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
    public DbSet<WorkspaceStatus> WorkspaceStatuses => Set<WorkspaceStatus>();
    public DbSet<TeamIssue> TeamIssues => Set<TeamIssue>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
