using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.IdentityModel.Tokens;
using WebApp.Common.Models;
using WebApp.Hubs.Common;
using WebApp.Hubs.Features.Chats;
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
                if (
                    !string.IsNullOrEmpty(accessToken)
                    && context.HttpContext.Request.Path.StartsWithSegments("/hubs/chat")
                )
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
builder.Services.AddHostedService<ChatBroadcastService>();

builder.Services.AddSignalR().AddMessagePackProtocol();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat");

// var httpClient = app.Services.GetRequiredService<IApiHttpClientFactory>().CreateClient();
// for (var i = 0; i != 1e3; ++i)
// {
//     var response = await httpClient
//         .GetAsync(
//             "api/internals/get-user-chat-ids/v1?userId=356b2b9e-9ccd-4935-8b93-e1a7ae18b824",
//             app.Lifetime.ApplicationStopping
//         )
//         .ConfigureAwait(false);
//     var list = await response
//         .Content.ReadFromJsonAsync<List<string>>(app.Lifetime.ApplicationStopping)
//         .ConfigureAwait(false);
// }
// httpClient.Dispose();

app.Run();
