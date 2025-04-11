using NATS.Client.Core;
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
        serviceCollection.AddSingleton(provider =>
        {
            var factory = provider.GetRequiredService<INatsConnectionFactory>();
            return factory.CreateNatsConnection();
        });
        serviceCollection.AddSingleton<INatsClient>(provider => provider.GetRequiredService<INatsConnection>());
        return serviceCollection;
    }
}
