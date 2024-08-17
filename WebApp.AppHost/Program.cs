using Microsoft.Extensions.Configuration;
using WebApp.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mailerOptions =
    builder.Configuration.GetSection(MailerOptions.Section).Get<MailerOptions>()
    ?? throw new InvalidOperationException("MailerOptions must be configured");

builder.AddContainer("nats", "nats", "latest").WithDockerfile("../").WithContainerRuntimeArgs("--network=host");

builder
    .AddContainer(mailerOptions.Name, mailerOptions.Image, mailerOptions.Tag)
    .WithDockerfile(mailerOptions.DockerContextPath)
    .WithContainerRuntimeArgs("--network=host");

builder.AddProject<Projects.WebApp_Api_V1>("api-v1");

builder.Build().Run();
