using WebApp.Infrastructure.Nats;
using WebApp.Infrastructure.Nats.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNATS(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<NatsOptions>()
            .BindConfiguration(NatsOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        serviceCollection.AddSingleton<INatsConnectionFactory, NatsConnectionFactory>();
        serviceCollection.AddScoped(provider =>
        {
            return provider.GetRequiredService<INatsConnectionFactory>().CreateNatsConnection();
        });
        return serviceCollection;
    }
}
