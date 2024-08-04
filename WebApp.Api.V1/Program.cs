using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using NATS.Client.Core;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using WebApp.Api.V1.Commons;
using WebApp.Api.V1.Commons.Converters;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Mails.Abstractions;
using WebApp.Infrastructure.Nats.Abstractions;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var persistenceOptions =
    builder.Configuration.GetRequiredSection(PersistenceOptions.Section).Get<PersistenceOptions>()
    ?? throw new InvalidOperationException("PersistenceOptions must be configured");

var natsOptions =
    builder.Configuration.GetRequiredSection(NatsOptions.Section).Get<NatsOptions>()
    ?? throw new InvalidOperationException("NatsOptions must be configured");

builder.AddServiceDefaults();

var natsOpts = NatsOpts.Default with
{
    Url = natsOptions.Url,
    AuthOpts = NatsAuthOpts.Default with { Username = natsOptions.Username, Password = natsOptions.Password }
};
builder.Services.AddScoped<INatsConnection>(provider => new NatsConnection(natsOpts));
builder
    .Services.AddOptions<NatsOptions>()
    .BindConfiguration(NatsOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();
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
    x.SerializerOptions.Converters.Add(new OrderableArrayJsonConverter());
    x.SerializerOptions.Converters.Add(GuidToBase64JsonConverter.Instance);
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<UserId>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<WorkspaceId>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<TeamId>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<IssueId>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<IssueCommentId>());
    x.SerializerOptions.TypeInfoResolverChain.Add(ApiJsonSerializerContext.Default);
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
        config.Binding.ValueParserFor<Orderable[]>(OrderableArrayJsonConverter.ValueParser);
        config.Binding.ValueParserFor<Guid>(GuidToBase64JsonConverter.ValueParser);
        config.Binding.ValueParserFor<UserId>(EntityGuidJsonConverter<UserId>.ValueParser);
        config.Binding.ValueParserFor<WorkspaceId>(EntityGuidJsonConverter<WorkspaceId>.ValueParser);
        config.Binding.ValueParserFor<TeamId>(EntityGuidJsonConverter<TeamId>.ValueParser);
        config.Binding.ValueParserFor<IssueId>(EntityGuidJsonConverter<IssueId>.ValueParser);
        config.Binding.ValueParserFor<IssueCommentId>(EntityGuidJsonConverter<IssueCommentId>.ValueParser);
        config.Binding.ValueParserFor<TeamRoleId>(EntityIdJsonConverter<TeamRoleId, int>.ValueParser);
        config.Binding.ValueParserFor<UserId?>(NullableEntityGuidJsonConverter<UserId>.ValueParser);
        config.Binding.ValueParserFor<WorkspaceId?>(NullableEntityGuidJsonConverter<WorkspaceId>.ValueParser);
        config.Binding.ValueParserFor<TeamId?>(NullableEntityGuidJsonConverter<TeamId>.ValueParser);
        config.Binding.ValueParserFor<IssueId?>(NullableEntityGuidJsonConverter<IssueId>.ValueParser);
        config.Binding.ValueParserFor<IssueCommentId?>(NullableEntityGuidJsonConverter<IssueCommentId>.ValueParser);
        config.Binding.ValueParserFor<TeamRoleId?>(NullableEntityIdJsonConverter<TeamRoleId, int>.ValueParser);
    }
);
app.MapDefaultEndpoints();
app.UseJobQueues(options =>
{
    options.MaxConcurrency = 4;
    options.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

app.Run();
