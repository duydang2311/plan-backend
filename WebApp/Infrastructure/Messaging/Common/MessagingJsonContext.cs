using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Messaging.Common;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(ChatMessageCreatedEvent))]
[JsonSerializable(typeof(IssueCommentCreatedEvent))]
[JsonSerializable(typeof(IssueCreatedEvent))]
[JsonSerializable(typeof(ProjectCreatedEvent))]
[JsonSerializable(typeof(ProjectMemberInvitedEvent))]
internal partial class MessagingJsonContext : JsonSerializerContext;
