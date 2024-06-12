using WebApp.SharedKernel.Jwts;
using WebApp.SharedKernel.Jwts.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJwtService, AsymmetricJwtService>();
        return serviceCollection;
    }
}
