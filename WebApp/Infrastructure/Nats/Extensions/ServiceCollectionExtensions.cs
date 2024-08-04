using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Infrastructure.Nats.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    private static NatsOpts? natsOpts;

    public static IServiceCollection AddNATS(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<NatsOptions>()
            .BindConfiguration(NatsOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        serviceCollection.AddScoped<INatsConnection>(provider =>
        {
            if (natsOpts is null)
            {
                var options = provider.GetRequiredService<IOptions<NatsOptions>>().Value;
                natsOpts = NatsOpts.Default with
                {
                    Url = options.Url,
                    AuthOpts = NatsAuthOpts.Default with { Username = options.Username, Password = options.Password }
                };
            }
            return new NatsConnection(natsOpts);
        });
        return serviceCollection;
    }
}
