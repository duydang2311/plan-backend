using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using WebApp.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mailerOptions =
    builder.Configuration.GetSection(MailerOptions.Section).Get<MailerOptions>()
    ?? throw new ValidationException("MailerOptions is required");
Validator.ValidateObject(mailerOptions, new ValidationContext(mailerOptions));

builder.AddContainer("nats", "nats", "latest").WithDockerfile("../").WithContainerRuntimeArgs("--network=host");

builder
    .AddContainer(mailerOptions.Name, mailerOptions.Image, mailerOptions.Tag)
    .WithDockerfile(mailerOptions.DockerContextPath)
    .WithContainerRuntimeArgs("--network=host")
    .WithEnvironment("RESEND_API_KEY", mailerOptions.ResendApiKey)
    .WithEnvironment("NATS_URL", mailerOptions.NatsUrl)
    .WithEnvironment("NATS_USER", mailerOptions.NatsUser)
    .WithEnvironment("NATS_PASSWORD", mailerOptions.NatsPassword);

builder.AddProject<Projects.WebApp_Api_V1>("api-v1");

builder.Build().Run();
