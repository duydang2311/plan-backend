using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<JobRecord> JobRecords => Set<JobRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
