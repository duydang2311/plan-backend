using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApp.Domain.Constants;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

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
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.Configure(options);
        return new AppDbContext(builder.Options);
    }
}
