using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using FastEndpoints;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using WebApp.Api.V1.Commons.Converters;
using WebApp.SharedKernel.Mails.Abstractions;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;
using WebApp.SharedKernel.Persistence.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var persistenceOptions =
    builder.Configuration.GetRequiredSection(PersistenceOptions.Section).Get<PersistenceOptions>()
    ?? throw new InvalidOperationException("PersistenceOptions must be configured");

builder.AddServiceDefaults();
builder
    .Services.AddOptions<FastEndpointsOptions>()
    .BindConfiguration(FastEndpointsOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder
    .Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder
    .Services.AddOptions<MailOptions>()
    .BindConfiguration(MailOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder
    .Services.AddOptions<PersistenceOptions>()
    .BindConfiguration(PersistenceOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>();
        if (jwtOptions is not null)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuers = jwtOptions.ValidIssuers,
                ValidAudiences = jwtOptions.ValidAudiences,
                IssuerSigningKey = new X509SecurityKey(
                    X509Certificate2.CreateFromEncryptedPemFile(
                        jwtOptions.CertificateFilePath,
                        jwtOptions.KeyPassword,
                        jwtOptions.KeyFilePath
                    )
                )
            };
        }
    });

builder.Services.AddPersistence(persistenceOptions).AddHashers().AddJwts().AddMails().AddAuthorization();
builder.Services.Configure<JsonOptions>(x =>
{
    x.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    x.SerializerOptions.Converters.Add(GuidToBase64JsonConverter.Instance);
    x.SerializerOptions.Converters.Add(new UserIdJsonConverter());
    x.SerializerOptions.Converters.Add(new WorkspaceIdJsonConverter());
    x.SerializerOptions.Converters.Add(new TeamIdJsonConverter());
});
builder.Services.AddJobQueues<JobRecord, JobStorageProvider>();
builder.Services.AddFastEndpoints(
    (options) =>
    {
        var fastEndpointsOptions = builder
            .Configuration.GetRequiredSection(FastEndpointsOptions.Section)
            .Get<FastEndpointsOptions>();
        options.DisableAutoDiscovery = true;
        options.Assemblies = fastEndpointsOptions?.Assemblies?.Select(Assembly.Load).ToArray() ?? [];
    }
);

builder.Services.AddMassTransit(
    (x) =>
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.UsingInMemory(
            (context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            }
        );
        x.AddMediator(cfg =>
        {
            cfg.AddConsumers(Assembly.Load("WebApp"));
        });
    }
);

var app = builder.Build();
app.UseDefaultExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(
    (config) =>
    {
        config.Endpoints.RoutePrefix = "api";
        config.Versioning.Prefix = "v";
        config.Versioning.PrependToRoute = false;
        config.Errors.UseProblemDetails(config =>
        {
            config.IndicateErrorCode = true;
        });
        config.Errors.ProducesMetadataType = typeof(ProblemDetails);
        config.Binding.ValueParserFor<Guid>(GuidToBase64JsonConverter.ValueParser);
        config.Binding.ValueParserFor<UserId>(UserIdJsonConverter.ValueParser);
        config.Binding.ValueParserFor<WorkspaceId>(WorkspaceIdJsonConverter.ValueParser);
        config.Binding.ValueParserFor<TeamId>(TeamIdJsonConverter.ValueParser);
    }
);
app.MapDefaultEndpoints();
app.UseJobQueues(options =>
{
    options.MaxConcurrency = 4;
    options.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

app.Run();
