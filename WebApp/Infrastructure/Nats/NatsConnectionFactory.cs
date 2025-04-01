using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Infrastructure.Nats.Abstractions;

namespace WebApp.Infrastructure.Nats;

public sealed class NatsConnectionFactory(IOptions<NatsOptions> options) : INatsConnectionFactory
{
    static NatsOpts? natsOpts;

    public INatsConnection CreateNatsConnection()
    {
        if (natsOpts is null)
        {
            var value = options.Value;
            natsOpts = NatsOpts.Default with
            {
                Url = value.Url,
                AuthOpts = NatsAuthOpts.Default with { Username = value.Username, Password = value.Password },
            };
        }
        return new NatsConnection(natsOpts);
    }
}
