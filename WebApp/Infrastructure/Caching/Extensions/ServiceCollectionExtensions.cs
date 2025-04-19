using WebApp.Infrastructure.Caching;
using WebApp.Infrastructure.Caching.Common;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPermissionCache, PermissionCache>();
        return serviceCollection;
    }
}
