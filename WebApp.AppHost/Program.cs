var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddContainer("nats", "nats", "latest")
    .WithDockerfile("../")
    .WithEndpoint(port: 4222, targetPort: 4222, name: "nats", scheme: "tcp")
    .WithEndpoint(port: 4223, targetPort: 4223, name: "nats-ws", scheme: "tcp")
    .WithEndpoint(port: 8222, targetPort: 8222, name: "nats-monitor", scheme: "http");

builder.AddProject<Projects.WebApp_Api_V1>("api-v1");

builder.Build().Run();
