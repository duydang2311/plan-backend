using System.Text.Json;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;

namespace WebApp.Features.Messaging;

public sealed class IssueCommentCreatedHandler : IEventHandler<IssueCommentCreated>
{
    public async Task HandleAsync(IssueCommentCreated eventModel, CancellationToken ct)
    {
        var nats = eventModel.ServiceProvider.GetRequiredService<INatsConnection>();
        var options = eventModel.ServiceProvider.GetRequiredService<IOptions<JsonOptions>>();
        await nats.PublishAsync(
                $"issues.{eventModel.IssueComment.IssueId.ToBase64String()}.comments.created",
                JsonSerializer.Serialize(
                    new { issueCommentId = eventModel.IssueComment.Id.Value, },
                    options.Value.SerializerOptions
                ),
                cancellationToken: ct
            )
            .ConfigureAwait(false);
    }
}
