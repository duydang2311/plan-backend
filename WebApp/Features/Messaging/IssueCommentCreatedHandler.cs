// using System.Text.Json;
// using FastEndpoints;
// using Microsoft.AspNetCore.Http.Json;
// using Microsoft.Extensions.Options;
// using NATS.Client.Core;
// using WebApp.Domain.Entities;
// using WebApp.Domain.Events;

// namespace WebApp.Features.Messaging;

// public sealed class IssueCommentCreatedHandler(
//     IServiceScopeFactory serviceScopeFactory,
//     ILogger<IssueCommentCreatedHandler> logger
// ) : IEventHandler<IssueCommentCreated>
// {
//     public async Task HandleAsync(IssueCommentCreated eventModel, CancellationToken ct)
//     {
//         await using var scope = serviceScopeFactory.CreateAsyncScope();
//         var nats = scope.ServiceProvider.GetRequiredService<INatsConnection>();
//         var options = scope.ServiceProvider.GetRequiredService<IOptions<JsonOptions>>();
//         try
//         {
//             await nats.PublishAsync(
//                     $"issues.{eventModel.IssueAudit.IssueId.ToBase64String()}.comments.created",
//                     JsonSerializer.Serialize(
//                         new { issueAuditId = eventModel.IssueAudit.Id },
//                         options.Value.SerializerOptions
//                     ),
//                     cancellationToken: ct
//                 )
//                 .ConfigureAwait(false);
//         }
//         catch (NatsException e)
//         {
//             logger.LogError(e, "Failed to publish comments.created message");
//             throw;
//         }
//     }
// }
