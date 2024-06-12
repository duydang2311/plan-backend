using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApp.SharedKernel.Persistence;
using WebApp.SharedKernel.Persistence.Abstractions;

namespace WebApp.Host.Persistence;

public sealed class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true
            )
            .Build();

        var options =
            configuration.GetSection(PersistenceOptions.Section).Get<PersistenceOptions>()
            ?? throw new InvalidOperationException("PersistenceOptions must be configured");
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(
                    options.ConnectionString,
                    builder => builder.UseNodaTime().MigrationsAssembly(options.MigrationsAssembly)
                )
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options
        );
    }
}
