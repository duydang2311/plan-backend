using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using WebApp.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var mailerOptions =
    builder.Configuration.GetSection(MailerOptions.Section).Get<MailerOptions>()
    ?? throw new ValidationException("MailerOptions is required");
Validator.ValidateObject(mailerOptions, new ValidationContext(mailerOptions));

var nats = builder
    .AddDockerfile("nats", "../")
    .WithEndpoint(4223, 4223, "ws")
    .WithEndpoint(4222, 4222, "nats", "nats")
    .WithEndpoint(8222, 8222, "http");

// var mailer = builder
//     .AddDockerfile(mailerOptions.Name, mailerOptions.DockerContextPath)
//     .WaitFor(nats)
//     .WithContainerRuntimeArgs("--network=host")
//     .WithEnvironment("RESEND_API_KEY", mailerOptions.ResendApiKey)
//     .WithEnvironment("NATS_URL", mailerOptions.NatsUrl)
//     .WithEnvironment("NATS_USER", mailerOptions.NatsUser)
//     .WithEnvironment("NATS_PASSWORD", mailerOptions.NatsPassword);

builder.AddProject<Projects.WebApp_Api_V1>("api-v1");

builder.Build().Run();
