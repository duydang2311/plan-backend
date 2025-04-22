using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;
using Wolverine.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddPooledDbContextFactory<AppDbContext>(
            (provider, builder) => Configure(builder, provider.GetRequiredService<IOptions<PersistenceOptions>>().Value)
        );
        serviceCollection.AddDbContextPool<AppDbContext>(
            (provider, builder) => Configure(builder, provider.GetRequiredService<IOptions<PersistenceOptions>>().Value)
        );
        serviceCollection.AddDbContextWithWolverineIntegration<AppDbContext>(
            (provider, builder) =>
                Configure(builder, provider.GetRequiredService<IOptions<PersistenceOptions>>().Value),
            "wolverine"
        );
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
            .UseProjectables()
#if DEBUG
            .EnableSensitiveDataLogging()
#endif
            .EnableDetailedErrors()
            .UseExceptionProcessor();
    }
}
