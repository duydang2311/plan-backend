using WebApp.Common.Jwts;
using WebApp.Common.Jwts.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJwtService, AsymmetricJwtService>();
        return serviceCollection;
    }
}
