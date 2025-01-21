#pragma warning disable EXTEXP0018

using System.Reflection;
using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using WebApp.Api.V1.Common.Authentications;
using WebApp.Api.V1.Common.Converters;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Mails.Abstractions;
using WebApp.Infrastructure.Nats.Abstractions;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;
using WebApp.Infrastructure.Storages.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var persistenceOptions =
    builder.Configuration.GetRequiredSection(PersistenceOptions.Section).Get<PersistenceOptions>()
    ?? throw new InvalidOperationException("PersistenceOptions must be configured");

var cloudinaryOptions =
    builder.Configuration.GetRequiredSection(CloudinaryOptions.Section).Get<CloudinaryOptions>()
    ?? throw new InvalidOperationException("CloudinaryOptions must be configured");

builder.AddServiceDefaults();

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
    .Services.AddOptions<CloudinaryOptions>()
    .BindConfiguration(CloudinaryOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder
    .Services.AddAuthentication()
    .AddScheme<BasicAuthenticationSchemeOptions, BasicAuthenticationSchemeHandler>(
        BasicAuthenticationSchemeOptions.DefaultScheme,
        options => { }
    );

// builder.Services
// .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
//     var jwtOptions = builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>();
//     if (jwtOptions is not null)
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuers = jwtOptions.ValidIssuers,
//             ValidAudiences = jwtOptions.ValidAudiences,
//             IssuerSigningKey = new X509SecurityKey(
//                 X509Certificate2.CreateFromEncryptedPemFile(
//                     jwtOptions.CertificateFilePath,
//                     jwtOptions.KeyPassword,
//                     jwtOptions.KeyFilePath
//                 )
//             )
//         };
//     }
// });

builder
    .Services.AddPersistence(persistenceOptions)
    .AddHashers()
    .AddJwts()
    .AddMails()
    .AddAuthorization()
    .AddNATS()
    .AddStorage(cloudinaryOptions);
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
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<TeamRoleId, int>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<TeamInvitationId>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<StatusId, long>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<RefreshToken>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<ProjectId>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<SessionToken>());
    x.SerializerOptions.Converters.Add(new PatchableJsonConverter());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<WorkspaceMemberId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<WorkspaceInvitationId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<RoleId, int>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<ProjectMemberId, long>());
    x.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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

builder.Services.AddHybridCache();

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
        config.Binding.JsonExceptionTransformer = ex =>
        {
            return new FluentValidation.Results.ValidationFailure(
                propertyName: ex.Path,
                errorMessage: ex.InnerException?.Message ?? ex.Message
            )
            {
                ErrorCode = "bad_json"
            };
        };
        config.Binding.ValueParserFor<Orderable[]>(OrderableArrayJsonConverter.ValueParser);
        config.Binding.ValueParserFor<Guid>(GuidToBase64JsonConverter.ValueParser);
        config.Binding.ValueParserFor<UserId>(EntityGuidJsonConverter<UserId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<WorkspaceId>(EntityGuidJsonConverter<WorkspaceId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<TeamId>(EntityGuidJsonConverter<TeamId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<IssueId>(EntityGuidJsonConverter<IssueId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<IssueCommentId>(
            EntityGuidJsonConverter<IssueCommentId>.ValueParser,
            handleNull: true
        );
        config.Binding.ValueParserFor<TeamInvitationId>(
            EntityGuidJsonConverter<TeamInvitationId>.ValueParser,
            handleNull: true
        );
        config.Binding.ValueParserFor<RefreshToken>(
            EntityGuidJsonConverter<RefreshToken>.ValueParser,
            handleNull: true
        );
        config.Binding.ValueParserFor<ProjectId>(EntityGuidJsonConverter<ProjectId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<SessionToken>(
            EntityGuidJsonConverter<SessionToken>.ValueParser,
            handleNull: true
        );
        config.Binding.ValueParserFor<WorkspaceMemberId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new WorkspaceMemberId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<WorkspaceInvitationId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new WorkspaceInvitationId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<RoleId>(
            input => EntityIdValueParsers.ParseInt(input, static value => new RoleId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<ProjectMemberId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new ProjectMemberId { Value = value }),
            handleNull: true
        );
    }
);
app.MapDefaultEndpoints();
app.UseJobQueues(options =>
{
    options.MaxConcurrency = 4;
    options.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

app.Run();
