using Casbin.Persist.Adapter.EFCore;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence;

public sealed class AppDbContext(DbContextOptions options) : CasbinDbContext<int>(options, tableName: "policies")
{
    public DbSet<User> Users => Set<User>();
    public DbSet<JobRecord> JobRecords => Set<JobRecord>();
    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
