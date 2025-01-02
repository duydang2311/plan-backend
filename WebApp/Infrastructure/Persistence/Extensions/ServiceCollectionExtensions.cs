using Casbin;
using Casbin.Persist;
using Casbin.Persist.Adapter.EFCore;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection serviceCollection,
        PersistenceOptions persistenceOptions
    )
    {
        serviceCollection.AddPooledDbContextFactory<AppDbContext>((builder) => Configure(builder, persistenceOptions));
        serviceCollection.AddDbContextPool<AppDbContext>((builder) => Configure(builder, persistenceOptions));
        serviceCollection.AddScoped<IAdapter>(provider => new EFCoreAdapter<int>(
            provider.GetRequiredService<AppDbContext>()
        ));
        serviceCollection.AddScoped<IEnforcer>(provider => new Enforcer(
            "Resources/casbin_model.conf",
            provider.GetRequiredService<IAdapter>()
        ));
        return serviceCollection;
    }

    public static void Configure(this DbContextOptionsBuilder builder, PersistenceOptions persistenceOptions)
    {
        builder
            .UseNpgsql(
                persistenceOptions.ConnectionString,
                builder =>
                    builder
                        .MapEnum<IssueAuditAction>("issue_audit_action")
                        .UseNodaTime()
                        .MigrationsAssembly(persistenceOptions.MigrationsAssembly)
            )
            .UseSnakeCaseNamingConvention()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
            .EnableSensitiveDataLogging()
#endif
            .EnableDetailedErrors()
            .UseExceptionProcessor();
    }
}
