using System.Drawing;
using Casbin;
using Casbin.Model;
using Casbin.Persist;
using Casbin.Persist.Adapter.EFCore;
using Casbin.Persist.Adapter.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Casbins.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCasbin(this IServiceCollection serviceCollection, CasbinOptions options)
    {
        serviceCollection.AddDbContext<CasbinDbContext<int>>(builder =>
            builder
                .UseNpgsql(options.ConnectionString)
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .EnableDetailedErrors()
        );
        serviceCollection.AddScoped<IAdapter, EFCoreAdapter<int>>();
        serviceCollection.AddScoped<IEnforcer>(provider => new Enforcer(
            options.ModelPath,
            provider.GetRequiredService<IAdapter>()
        ));
        return serviceCollection;
    }
}
