using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Persistence;
using WebApp.SharedKernel.Persistence.Abstractions;

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

    private static void Configure(DbContextOptionsBuilder builder, PersistenceOptions persistenceOptions)
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
            .EnableDetailedErrors();
    }
}
