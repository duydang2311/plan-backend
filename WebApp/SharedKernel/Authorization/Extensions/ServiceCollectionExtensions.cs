using WebApp.SharedKernel.Authorization;
using WebApp.SharedKernel.Authorization.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEnforcer, Enforcer>();
        return serviceCollection;
    }
}
