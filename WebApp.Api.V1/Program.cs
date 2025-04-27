using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FastEndpoints;
using JasperFx.Core;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using NATS.Client.Core;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Oakton;
using Oakton.Resources;
using WebApp.Api.V1.Common.Authentications;
using WebApp.Api.V1.Common.Converters;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Jwts.Common;
using WebApp.Infrastructure.Mails.Abstractions;
using WebApp.Infrastructure.Messaging;
using WebApp.Infrastructure.Nats.Abstractions;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;
using WebApp.Infrastructure.Storages.Abstractions;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

var persistenceOptions =
    builder.Configuration.GetRequiredSection(PersistenceOptions.Section).Get<PersistenceOptions>()
    ?? throw new InvalidOperationException("PersistenceOptions must be configured");

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
    .Services.AddOptions<R2Options>()
    .BindConfiguration(R2Options.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = SessionAuthenticationSchemeOptions.AuthenticationScheme;
        options.DefaultChallengeScheme = SessionAuthenticationSchemeOptions.AuthenticationScheme;
    })
    .AddScheme<SessionAuthenticationSchemeOptions, SessionAuthenticationSchemeHandler>(
        SessionAuthenticationSchemeOptions.AuthenticationScheme,
        null
    )
    .AddJwtBearer(options =>
    {
        var jwtOptions =
            builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JwtOptions must be configured");
        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtOptions.PublicKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = "WebApp",
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuerSigningKey = true,
        };
    });

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
    .Services.AddPersistence()
    .AddHashers()
    .AddMails()
    .AddAuthorization()
    .AddNATS()
    .AddStorage()
    .AddCaching()
    .AddJwts();
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
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<SessionId, string>());
    x.SerializerOptions.Converters.Add(new PatchableJsonConverter());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<WorkspaceMemberId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<WorkspaceInvitationId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<RoleId, int>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<ProjectMemberId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<ProjectMemberInvitationId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<NotificationId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<UserNotificationId, long>());
    x.SerializerOptions.Converters.Add(new EntityGuidJsonConverter<ChatId>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<ChatMessageId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<ResourceId, long>());
    x.SerializerOptions.Converters.Add(new EntityIdJsonConverter<StoragePendingUploadId, long>());
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

builder.Host.UseWolverine(a =>
{
    a.PersistMessagesWithPostgresql(persistenceOptions.ConnectionString, "wolverine");
    a.UseEntityFrameworkCoreTransactions();
    a.Policies.UseDurableLocalQueues();
    a.Policies.AutoApplyTransactions();
    a.Discovery.IncludeAssembly(typeof(ChatMessageCreatedHandler).Assembly);
    a.OnException<NatsException>()
        .RetryWithCooldown(512.Milliseconds(), 2048.Milliseconds(), 8192.Milliseconds())
        .Then.MoveToErrorQueue();
});

builder.Host.UseResourceSetupOnStartup();

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
            var bindEx = ex as JsonBindException;
            return new FluentValidation.Results.ValidationFailure(
                propertyName: ex.Path is null or "$" || ex.Path.StartsWith("$[")
                    ? bindEx?.FieldName ?? "$"
                    : ex.Path[2..],
                errorMessage: ex.InnerException?.Message ?? ex.Message
            )
            {
                ErrorCode = "bad_json",
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
        config.Binding.ValueParserFor<SessionId>(
            input => EntityIdValueParsers.ParseString(input, static value => new SessionId { Value = value }),
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
        config.Binding.ValueParserFor<ProjectMemberInvitationId>(
            input =>
                EntityIdValueParsers.ParseLong(input, static value => new ProjectMemberInvitationId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<NotificationId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new NotificationId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<UserNotificationId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new UserNotificationId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<ChatId>(EntityGuidJsonConverter<ChatId>.ValueParser, handleNull: true);
        config.Binding.ValueParserFor<ChatMessageId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new ChatMessageId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<ResourceId>(
            input => EntityIdValueParsers.ParseLong(input, static value => new ResourceId { Value = value }),
            handleNull: true
        );
        config.Binding.ValueParserFor<StoragePendingUploadId>(
            input =>
                EntityIdValueParsers.ParseLong(input, static value => new StoragePendingUploadId { Value = value }),
            handleNull: true
        );
    }
);

app.UseJobQueues(options =>
{
    options.MaxConcurrency = 4;
    options.ExecutionTimeLimit = TimeSpan.FromSeconds(10);
});

// var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
// using var scope = scopeFactory.CreateScope();
// var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

// var tokenHandler = new JwtSecurityTokenHandler();
// var rsa = RSA.Create();
// rsa.ImportFromPem(app.Services.GetRequiredService<IOptions<JwtOptions>>().Value.PrivateKey);

// var tokenDescriptor = new SecurityTokenDescriptor
// {
//     Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "HUBS"), new Claim(ClaimTypes.Role, "HUBS")]),
//     Expires = DateTime.UtcNow.AddYears(4),
//     Issuer = app.Services.GetRequiredService<IOptions<JwtOptions>>().Value.Issuer,
//     Audience = "WebApp",
//     SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256),
// };
// var token = tokenHandler.CreateToken(tokenDescriptor);
// Console.WriteLine("Token: " + tokenHandler.WriteToken(token));
// return 0;

// var rsa = RSA.Create(4096);
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(rsa.ExportPkcs8PrivateKeyPem()));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(rsa.ExportSubjectPublicKeyInfoPem()));
// return 0;

return await app.RunOaktonCommands(args).ConfigureAwait(false);
