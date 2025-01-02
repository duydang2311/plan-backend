using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
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
        return serviceCollection;
    }

    public static void Configure(this DbContextOptionsBuilder builder, PersistenceOptions persistenceOptions)
    {
        builder
            .UseNpgsql(
                persistenceOptions.ConnectionString,
                builder => builder.UseNodaTime().MigrationsAssembly(persistenceOptions.MigrationsAssembly)
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
