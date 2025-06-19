using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using WebApp.Hubs.Common;
using WebApp.Hubs.Features.Chats;
using WebApp.Hubs.Features.Hubs;
using WebApp.Hubs.Features.Notifications;
using WebApp.Infrastructure.Jwts.Common;
using WebApp.Infrastructure.Nats.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions =
    builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>()
    ?? throw new InvalidOperationException("JwtOptions must be configured");

var apiOptions =
    builder.Configuration.GetSection(ApiOptions.Section).Get<ApiOptions>()
    ?? throw new InvalidOperationException("ApiOptions must be configured");

builder
    .Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder
    .Services.AddOptions<NatsOptions>()
    .BindConfiguration(NatsOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder
    .Services.AddOptions<ApiOptions>()
    .BindConfiguration(ApiOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtOptions.PublicKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = "WebApp.Hubs",
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuerSigningKey = true,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/hub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
        };
    });

builder
    .Services.AddHttpClient(
        apiOptions.HttpClientName,
        client =>
        {
            client.BaseAddress = new Uri(apiOptions.BaseUrl);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HUBS");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                apiOptions.AccessToken
            );
        }
    )
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.DisableForUnsafeHttpMethods();
    });

builder.Services.AddSingleton<IApiHttpClientFactory, ApiHttpClientFactory>();

builder.Services.AddNATS();

builder.Services.AddSingleton<ChatHubService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ChatHubService>());
builder.Services.AddSingleton<IHubService, ChatHubService>(provider => provider.GetRequiredService<ChatHubService>());

builder.Services.AddSingleton<NotificationHubService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<NotificationHubService>());
builder.Services.AddSingleton<IHubService, NotificationHubService>(provider =>
    provider.GetRequiredService<NotificationHubService>()
);

builder.Services.AddSignalR().AddMessagePackProtocol();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:5173", "http://localhost:3000")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MainHub>("/hub");

app.Run();
