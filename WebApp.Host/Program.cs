using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using WebApp.Host.Commons;
using WebApp.SharedKernel.Mails.Abstractions;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;
using WebApp.SharedKernel.Persistence.Abstractions;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddPersistence(
    builder.Configuration.GetRequiredSection(PersistenceOptions.Section).Get<PersistenceOptions>()
        ?? throw new InvalidOperationException("PersistenceOptions must be configured")
);
builder.Services.AddHashers();
builder.Services.AddJwts();
builder.Services.AddMails();
builder.Services.AddAuthorization();
builder.Services.Configure<JsonOptions>(x => x.SerializerOptions.Converters.Add(new GuidToBase64JsonConverter()));
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

var app = builder.Build();
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
    }
);
app.UseJobQueues(options =>
{
    options.MaxConcurrency = 4;
    options.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

app.Run();
