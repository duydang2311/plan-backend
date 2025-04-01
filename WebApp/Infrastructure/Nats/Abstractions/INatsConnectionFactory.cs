using NATS.Client.Core;

namespace WebApp.Infrastructure.Nats.Abstractions;

public interface INatsConnectionFactory
{
    INatsConnection CreateNatsConnection();
}
