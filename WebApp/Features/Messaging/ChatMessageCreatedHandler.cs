using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Features.Messaging;

public sealed class ChatMessageCreatedHandler(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ChatMessageCreatedHandler> logger,
    IOptions<JsonOptions> options
) : ICommandHandler<ChatMessageCreated>
{
    public async Task ExecuteAsync(ChatMessageCreated command, CancellationToken ct)
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var scope = serviceScopeFactory.CreateAsyncScope();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        var nats = scope.ServiceProvider.GetRequiredService<INatsConnection>();
        try
        {
            await nats.PublishAsync(
                    $"chats.{command.ChatId.ToBase64String()}.messages.created",
                    JsonSerializer.Serialize(
                        new { chatMessageId = command.ChatMessageId },
                        options.Value.SerializerOptions
                    ),
                    cancellationToken: ct
                )
                .ConfigureAwait(false);
        }
        catch (NatsException e)
        {
            logger.LogError(e, "Failed to publish chat message created event to NATS");
            throw;
        }
    }
}
