using WebApp.Infrastructure.Jwts;
using WebApp.Infrastructure.Jwts.Common;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwts(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        serviceCollection.AddSingleton<IJwtService, JwtService>();
        return serviceCollection;
    }
}
