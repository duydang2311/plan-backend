using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence.Abstractions;

namespace WebApp.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
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
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<WorkspaceStatus> WorkspaceStatuses => Set<WorkspaceStatus>();
    public DbSet<TeamIssue> TeamIssues => Set<TeamIssue>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<IssueAssignee> IssueAssignees => Set<IssueAssignee>();
    public DbSet<WorkspaceInvitation> WorkspaceInvitations => Set<WorkspaceInvitation>();
    public DbSet<IssueAudit> IssueAudits => Set<IssueAudit>();
    public DbSet<ProjectTeam> ProjectTeams => Set<ProjectTeam>();
    public DbSet<ProjectMemberInvitation> ProjectMemberInvitations => Set<ProjectMemberInvitation>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserFriend> UserFriends => Set<UserFriend>();
    public DbSet<UserFriendRequest> UserFriendRequests => Set<UserFriendRequest>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatMember> ChatMembers => Set<ChatMember>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<WorkspaceResource> WorkspaceResources => Set<WorkspaceResource>();
    public DbSet<ProjectResource> ProjectResources => Set<ProjectResource>();
    public DbSet<StoragePendingUpload> StoragePendingUploads => Set<StoragePendingUpload>();
    public DbSet<ResourceFile> ResourceFiles => Set<ResourceFile>();
    public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();
    public DbSet<UserSocialLink> UserSocialLinks => Set<UserSocialLink>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<MilestoneStatus> MilestoneStatuses => Set<MilestoneStatus>();
    public DbSet<UserVerificationToken> UserVerificationTokens => Set<UserVerificationToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.HasPostgresExtension("unaccent");
        modelBuilder
            .HasDbFunction(
                typeof(CustomDbFunctions).GetMethod(nameof(CustomDbFunctions.ImmutableUnaccent), [typeof(string)])!
            )
            .HasName("immutable_unaccent")
            .HasSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
